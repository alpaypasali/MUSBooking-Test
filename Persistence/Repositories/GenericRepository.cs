using Application.Services;
using Infrastructure.Paging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Persistence.Repositories;

public class GenericRepository<TContext, TEntity> : IRepository<TEntity>
 where TContext : DbContext
 where TEntity : class
{
    private readonly TContext _context;

    public GenericRepository(TContext context)
    {
        _context = context;
    }

    public async Task<TEntity> GetByIdAsync(int id)
    {
        return await _context.Set<TEntity>().FindAsync(id);
    }

    public async Task<List<TEntity>> GetAllAsync()
    {
        return await _context.Set<TEntity>().ToListAsync();
    }
    public async Task<TEntity?> GetAsync(
    Expression<Func<TEntity, bool>> predicate,
    Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
    bool enableTracking = true,
    CancellationToken cancellationToken = default
)
    {
        IQueryable<TEntity> queryable = Query();
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        return await queryable.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<Paginate<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> queryable = Query();
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (predicate != null)
            queryable = queryable.Where(predicate);
        if (orderBy != null)
            return await orderBy(queryable).ToPaginateAsync(index, size, cancellationToken);
        return await queryable.ToPaginateAsync(index, size, cancellationToken);
    }



    public async Task<TEntity> AddAsync(TEntity entity)
    {

        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }



    public async Task<TEntity> UpdateAsync(TEntity entity)
    {

        _context.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<TEntity> DeleteAsync(TEntity entity)
    {

        _context.Remove(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<TEntity?> GetAsync(
       Expression<Func<TEntity, bool>> predicate,
       Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
       bool withDeleted = false,
       bool enableTracking = true,
       CancellationToken cancellationToken = default
   )
    {
        IQueryable<TEntity> queryable = Query();
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        return await queryable.FirstOrDefaultAsync(predicate, cancellationToken);
    }
    public async Task<bool> AnyAsync(
       Expression<Func<TEntity, bool>>? predicate = null,
       bool enableTracking = true,
       CancellationToken cancellationToken = default
   )
    {
        IQueryable<TEntity> queryable = Query();
        if (predicate is not null)
            queryable = queryable.Where(predicate);
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        return await queryable.AnyAsync(cancellationToken);
    }


    public IQueryable<TEntity> Query() => _context.Set<TEntity>();
    public async Task<List<TEntity>> GetListAsync(
    Expression<Func<TEntity, bool>>? predicate = null,
    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
    Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
  
    bool enableTracking = true

)
    {
        IQueryable<TEntity> queryable = Query();
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
       
        if (predicate != null)
            queryable = queryable.Where(predicate);
        if (orderBy != null)
            return await orderBy(queryable).ToListAsync();
        return await queryable.ToListAsync();
    }



}



