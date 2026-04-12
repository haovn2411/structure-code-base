using Microsoft.EntityFrameworkCore;
using StructureCodeSolution.Domain.Abstractions.Repositories;
using StructureCodeSolution.Domain.Aggregates.Courses;

namespace StructureCodeSolution.Persistence.Repositories
{
    public class CourseRepository : RepositoryBase<Course, Guid>, ICourseRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public CourseRepository(ApplicationDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Courses
                .AsNoTracking()
                .AnyAsync(c => c.Name == name, cancellationToken);
        }

        public async Task<Video?> GetVideoByIdAsync(Guid courseId, Guid videoId, CancellationToken cancellationToken = default)
        {
            var video = await _dbContext.Videos
                .AsNoTracking()
                .FirstOrDefaultAsync(v =>
                    v.Id == videoId &&
                    EF.Property<Guid>(v, "CourseId") == courseId, cancellationToken);

            return video;
        }

        public IQueryable<Video> GetVideos(Guid courseId, string? searchTerm = null, CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Videos
                .AsNoTracking()
                .Where(v => EF.Property<Guid>(v, "CourseId") == courseId);
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(v => v.Title.Contains(searchTerm));
            }
            var videos = query
                .OrderBy(v => v.Order);
            return query;
        }
    }
}