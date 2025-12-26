using StructureCodeSolution.Domain.Abstractions;

namespace StructureCodeSolution.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDBContext _context;
        public UnitOfWork(ApplicationDBContext context)
            => _context = context;

        async ValueTask IAsyncDisposable.DisposeAsync()
            => await _context.DisposeAsync();

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
            => await _context.SaveChangesAsync();

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
            => await _context.Database.BeginTransactionAsync(cancellationToken);

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
            => await _context.Database.CommitTransactionAsync(cancellationToken);

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
            => await _context.Database.RollbackTransactionAsync(cancellationToken);
    }
}
