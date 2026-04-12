using AutoMapper;
using StructureCodeSolution.Application.Abstractions.Message;
using StructureCodeSolution.Application.Abstractions.Shared;
using StructureCodeSolution.Application.Usecases.V1.Queries.Courses.Abstracts;
using StructureCodeSolution.Domain.Abstractions.Repositories;

namespace StructureCodeSolution.Application.Usecases.V1.Queries.Courses
{
    internal class GetVideoByIdQueryHandler : IQueryHandler<Query.GetVideoByIdQuery, Response.VideoResponse>
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;

        public GetVideoByIdQueryHandler(ICourseRepository courseRepository, IMapper mapper)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
        }

        public async Task<Result<Response.VideoResponse>> Handle(Query.GetVideoByIdQuery request, CancellationToken cancellationToken)
        {
            var video = await _courseRepository.GetVideoByIdAsync(request.CourseId, request.VideoId, cancellationToken);

            if (video is null)
            {
                return Result.Failure<Response.VideoResponse>(new Error(
                    "Video.NotFound",
                    $"Course with id '{request.VideoId}' was not found"));
            }

            var response = _mapper.Map<Response.VideoResponse>(video);
            return Result.Success(response);
        }
    }
}