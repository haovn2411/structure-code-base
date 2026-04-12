using AutoMapper;
using StructureCodeSolution.Application.Abstractions.Message;
using StructureCodeSolution.Application.Abstractions.Shared;
using StructureCodeSolution.Application.Usecases.V1.Queries.Courses.Abstracts;
using StructureCodeSolution.Domain.Abstractions.Repositories;
using StructureCodeSolution.Domain.Aggregates.Courses;

namespace StructureCodeSolution.Application.Usecases.V1.Queries.Courses
{
    public class GetCoursesQueryHandler : IQueryHandler<Query.GetAllCoursesQuery, PagedResult<Response.CourseResponse>>
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;

        public GetCoursesQueryHandler(ICourseRepository courseRepository, IMapper mapper)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<Response.CourseResponse>>> Handle(Query.GetAllCoursesQuery request, CancellationToken cancellationToken)
        {
            var coursesQuery = _courseRepository.GetAll();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm.Trim().ToLower();
                coursesQuery = coursesQuery.Where(
                    c => c.Name.ToLower().Contains(searchTerm)
                    || (c.SummaryDescription != null
                        && c.SummaryDescription.ToLower().Contains(searchTerm)));
            }

            if (request.CategoryId.HasValue)
            {
                coursesQuery = coursesQuery.Where(c => c.CategoryId == request.CategoryId.Value);
            }

            if (request.LevelId.HasValue)
            {
                coursesQuery = coursesQuery.Where(c => c.LevelId == request.LevelId.Value);
            }

            var courses = await PagedResult<Course>
                .CreateAsync(coursesQuery, request.PageIndex, request.PageSize);

            var result = _mapper.Map<PagedResult<Response.CourseResponse>>(courses);

            return Result.Success(result);
        }
    }
}