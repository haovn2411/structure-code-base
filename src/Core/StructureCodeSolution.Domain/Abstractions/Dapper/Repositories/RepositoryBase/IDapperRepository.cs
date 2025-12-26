namespace StructureCodeSolution.Domain.Abstractions.Dapper.Repositories.RepositoryBase
{
    public interface IDapperRepository<T>
        where T : class
    {
        Task<T?> GetByIdAsync(Guid id);
    }
}
