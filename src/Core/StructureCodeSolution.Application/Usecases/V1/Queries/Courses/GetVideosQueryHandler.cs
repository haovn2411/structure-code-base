using AutoMapper;
using StructureCodeSolution.Application.Abstractions.Message;
using StructureCodeSolution.Application.Abstractions.Shared;
using StructureCodeSolution.Application.Usecases.V1.Queries.Courses.Abstracts;
using StructureCodeSolution.Domain.Abstractions.Repositories;
using StructureCodeSolution.Domain.Aggregates.Courses;

namespace StructureCodeSolution.Application.Usecases.V1.Queries.Courses
{
    public class GetVideosQueryHandler : IQueryHandler<Query.GetVideosQuery, PagedResult<Response.VideoResponse>>
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;

        public GetVideosQueryHandler(ICourseRepository courseRepository, IMapper mapper)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
        }

        public async Task<Result<PagedResult<Response.VideoResponse>>> Handle(Query.GetVideosQuery request, CancellationToken cancellationToken)
        {
            var videosQuery = _courseRepository.GetVideos(
                request.CourseId,
                request.SearchTerm,
                cancellationToken);

            var videos = await PagedResult<Video>
                .CreateAsync(videosQuery, request.PageIndex, request.PageSize);

            var result = _mapper.Map<PagedResult<Response.VideoResponse>>(videos);

            return Result.Success(result);
        }
    }
}