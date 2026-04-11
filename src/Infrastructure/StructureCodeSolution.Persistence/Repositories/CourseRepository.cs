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
    }
}