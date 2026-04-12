using StructureCodeSolution.Domain.Abstractions.Repositories.RepositoryBase;
using StructureCodeSolution.Domain.Aggregates.Courses;

namespace StructureCodeSolution.Domain.Abstractions.Repositories
{
    public interface ICourseRepository : IRepositoryBase<Course, Guid>
    {
        Task<Video?> GetVideoByIdAsync(Guid courseId, Guid videoId, CancellationToken cancellationToken = default);

        IQueryable<Video> GetVideos(Guid courseId, string? searchTerm = null, CancellationToken cancellationToken = default);

        Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
    }
}