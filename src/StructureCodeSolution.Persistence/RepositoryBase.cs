using Microsoft.EntityFrameworkCore;
using StructureCodeSolution.Domain.Abstractions;
using StructureCodeSolution.Domain.Abstractions.Repositories.RepositoryBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StructureCodeSolution.Persistence
{
    public class RepositoryBase<TEntity, TKey> : IRepositoryBase<TEntity, TKey>, IDisposable
        where TEntity : EntityBase<TKey>
    {
        private readonly ApplicationDBContext _dbContext;

        public RepositoryBase(ApplicationDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public void Dispose() => _dbContext?.Dispose();

        public void Add(TEntity entity)
            => _dbContext.Add(entity);

        public void Delete(TEntity entity)
            => _dbContext.Set<TEntity>().Remove(entity);



        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>>? predicate = null, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> items = _dbContext.Set<TEntity>().AsNoTracking(); // Importance Always include AsNoTracking for Query Side
            if (includeProperties != null)
                foreach (var includeProperty in includeProperties)
                    items = items.Include(includeProperty);
            if (predicate is not null)
                items = items.Where(predicate);
            return items;
        }

        public async Task<TEntity> GetByIdAsync(TKey id, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includeProperties)
            => await GetAll(null, includeProperties)
            .AsTracking()
            .SingleOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);


        public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includeProperties)
            => await GetAll(null, includeProperties)
            .AsTracking()
            .SingleOrDefaultAsync(predicate, cancellationToken);

        public void RemoveMultiple(List<TEntity> entities)
            => _dbContext.RemoveRange(entities);

        public void Update(TEntity entity)
            => _dbContext.Update(entity);
    }
}
