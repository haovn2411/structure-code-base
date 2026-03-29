using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StructureCodeSolution.Application.Abstractions.Message;
using StructureCodeSolution.Application.Abstractions.Shared;
using StructureCodeSolution.Application.Usecases.V1.Queries.Courses.Abstracts;
using StructureCodeSolution.Domain.Abstractions.Repositories;

namespace StructureCodeSolution.Application.Usecases.V1.Queries.Courses
{
    public class GetAllCoursesQueryHandler : IQueryHandler<Query.GetAllCoursesQuery, Response.CourseListResponse>
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;

        public GetAllCoursesQueryHandler(ICourseRepository courseRepository, IMapper mapper)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
        }

        public async Task<Result<Response.CourseListResponse>> Handle(Query.GetAllCoursesQuery request, CancellationToken cancellationToken)
        {
            var query = _courseRepository.GetAll();

            // Apply filters
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                query = query.Where(c => 
                    c.Name.Contains(request.SearchTerm) || 
                    (c.SummaryDescription != null && c.SummaryDescription.Contains(request.SearchTerm)));
            }

            if (request.CategoryId.HasValue)
            {
                query = query.Where(c => c.CategoryId == request.CategoryId.Value);
            }

            if (request.LevelId.HasValue)
            {
                query = query.Where(c => c.LevelId == request.LevelId.Value);
            }

            // Get total count
            var totalCount = await query.CountAsync(cancellationToken);

            // Apply pagination
            var pageIndex = request.PageIndex <= 0 ? 1 : request.PageIndex;
            var pageSize = request.PageSize <= 0 ? 10 : request.PageSize > 100 ? 100 : request.PageSize;

            // ? Include Videos collection
            var courses = await query
                .Include("_videos")  // ? Explicit include
                .OrderByDescending(c => c.CreatedDate)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var courseResponses = _mapper.Map<List<Response.CourseResponse>>(courses);

            var response = new Response.CourseListResponse(
                courseResponses,
                pageIndex,
                pageSize,
                totalCount,
                pageIndex * pageSize < totalCount,
                pageIndex > 1);

            return Result.Success(response);
        }
    }
}
