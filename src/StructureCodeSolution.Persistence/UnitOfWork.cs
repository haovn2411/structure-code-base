using Microsoft.EntityFrameworkCore;
using StructureCodeSolution.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureCodeSolution.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDBContext _context;
        public UnitOfWork(ApplicationDBContext context)
            => _context = context;

        async ValueTask IAsyncDisposable.DisposeAsync()
            => await _context.DisposeAsync();

        public DbContext GetDbContext()
            => _context;

        public async Task SaveChangeAsync(CancellationToken cancellationToken = default)
            => await _context.SaveChangesAsync();
    }
}
