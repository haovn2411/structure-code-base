using AutoMapper;
using StructureCodeSolution.Application.Abstractions.Message;
using StructureCodeSolution.Application.Abstractions.Shared;
using StructureCodeSolution.Application.Usecases.V1.Queries.Courses.Abstracts;
using StructureCodeSolution.Domain.Abstractions.Repositories;

namespace StructureCodeSolution.Application.Usecases.V1.Queries.Courses
{
    public class GetCourseByIdQueryHandler : IQueryHandler<Query.GetCourseByIdQuery, Response.CourseResponse>
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;

        public GetCourseByIdQueryHandler(ICourseRepository courseRepository, IMapper mapper)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
        }

        public async Task<Result<Response.CourseResponse>> Handle(Query.GetCourseByIdQuery request, CancellationToken cancellationToken)
        {
            var course = await _courseRepository.GetByIdAsync(request.CourseId, cancellationToken);
            
            if (course is null)
            {
                return Result.Failure<Response.CourseResponse>(new Error(
                    "Course.NotFound",
                    $"Course with id '{request.CourseId}' was not found"));
            }

            var response = _mapper.Map<Response.CourseResponse>(course);
            return Result.Success(response);
        }
    }
}
