using StructureCodeSolution.Domain.Abstractions.Repositories.RepositoryBase;
using StructureCodeSolution.Domain.Aggregates.Courses;

namespace StructureCodeSolution.Domain.Abstractions.Repositories
{
    public interface ICourseRepository : IRepositoryBase<Course, Guid>
    {
        Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
    }
}
