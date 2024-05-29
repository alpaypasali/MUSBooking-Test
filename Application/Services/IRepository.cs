
using Infrastructure.Paging;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Services;


public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity> GetByIdAsync(int id);
    Task<List<TEntity>> GetAllAsync();
    Task<List<TEntity>> GetListAsync(
    Expression<Func<TEntity, bool>>? predicate = null,
    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
    Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,

    bool enableTracking = true

);
    Task<PaginatedList<TEntity>> GetPagedAsync(int pageNumber, int pageSize);
    Task<TEntity> AddAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task<TEntity> DeleteAsync(TEntity entity);
    Task<TEntity?> GetAsync(
       Expression<Func<TEntity, bool>> predicate,
       Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
       bool withDeleted = false,
       bool enableTracking = true,
       CancellationToken cancellationToken = default
   );

}
