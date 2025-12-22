using Microsoft.EntityFrameworkCore;

namespace StructureCodeSolution.Domain.Abstractions
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        Task SaveChangeAsync(CancellationToken cancellationToken = default);
        DbContext GetDbContext();
    }
}
