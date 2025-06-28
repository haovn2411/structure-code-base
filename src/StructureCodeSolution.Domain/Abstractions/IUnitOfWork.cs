using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureCodeSolution.Domain.Abstractions
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        Task SaveChangeAsync(CancellationToken cancellationToken =  default);
        DbContext GetDbContext();
    }
}
