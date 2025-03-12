using Microsoft.EntityFrameworkCore;
using ShiftSwift.Application.Common.Repository;
using ShiftSwift.Presistence.Context;

namespace ShiftSwift.Presistence.Common.Repository
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly ShiftSwiftDbContext _dbContext;
        public BaseRepository(ShiftSwiftDbContext dbContext) => _dbContext = dbContext;
        public IQueryable<TEntity> Entites() => _dbContext.Set<TEntity>();

        public async Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
            => await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync(cancellationToken);
        
        public async Task<int> CountAsync(IQueryable<TEntity> query, CancellationToken cancellationToken = default)
            => await query.CountAsync(cancellationToken);

        public Task<bool> DeleteAsync(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            return Task.FromResult(true);
        }

        public async Task<TEntity> AddEntityAsync(TEntity entity)
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
            return entity;
        }

        public Task<TEntity> UpdateAsync(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
            return Task.FromResult(entity);
        }
    }
}
