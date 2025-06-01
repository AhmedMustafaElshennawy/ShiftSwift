namespace ShiftSwift.Application.Common.Repository;

public interface IBaseRepository<TEntity> where TEntity : class
{
    IQueryable<TEntity> Entites();
    Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<int> CountAsync(IQueryable<TEntity> query, CancellationToken cancellationToken = default);
    Task<TEntity> AddEntityAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task<bool> DeleteAsync(TEntity entity);
}