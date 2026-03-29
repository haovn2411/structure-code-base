using AutoMapper;
using StructureCodeSolution.Application.Abstractions.Message;
using StructureCodeSolution.Application.Abstractions.Shared;
using StructureCodeSolution.Application.Usecases.V1.Queries.Courses.Abstracts;
using StructureCodeSolution.Domain.Abstractions.Repositories;

namespace StructureCodeSolution.Application.Usecases.V1.Queries.Courses
{
    public class GetCourseVideosQueryHandler : IQueryHandler<Query.GetCourseVideosQuery, Response.VideoListResponse>
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;

        public GetCourseVideosQueryHandler(ICourseRepository courseRepository, IMapper mapper)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
        }

        public async Task<Result<Response.VideoListResponse>> Handle(Query.GetCourseVideosQuery request, CancellationToken cancellationToken)
        {
            var course = await _courseRepository.GetByIdAsync(request.CourseId, cancellationToken);
            
            if (course is null)
            {
                return Result.Failure<Response.VideoListResponse>(new Error(
                    "Course.NotFound",
                    $"Course with id '{request.CourseId}' was not found"));
            }

            var videos = course.Videos.OrderBy(v => v.Order).ToList();
            var videoResponses = _mapper.Map<List<Response.VideoResponse>>(videos);
            var totalHours = (int)Math.Ceiling(course.Videos.Sum(v => v.Duration.TotalHours));

            var response = new Response.VideoListResponse(
                course.Id,
                course.Name,
                videoResponses,
                videos.Count,
                totalHours);

            return Result.Success(response);
        }
    }
}
