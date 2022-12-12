using Domain.Entities;
using Domain.IRepositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class Repository<T> : IRepository<T>
    where T : Base
{
    public readonly DbSet<T> _entities;
    private readonly ADbContext _dbContext;
    virtual protected IQueryable<T> _query { get => _entities; }

    public Repository(ADbContext aDbContext)
    {
        _dbContext = aDbContext;
        _entities = aDbContext.Set<T>();
    }

    public Task<T?> GetAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[]? includes = null)
    {
        IQueryable<T> query = Include(includes);
        return query.Where(r => r.IsActive).FirstOrDefaultAsync(predicate);
    }

    public Task<List<T>> GetListAsync(Expression<Func<T, bool>>? predicate = null, Expression<Func<T, object>>[]? includes = null)
    {
        IQueryable<T> query = Include(includes);
        query = query.Where(r => r.IsActive);
        return predicate != null ? query.Where(predicate).ToListAsync() :
          query.ToListAsync();
    }

    public Task<List<T>> GetDistinctListAsync(Expression<Func<T, bool>>? predicate = null, Expression<Func<T, object>>[]? includes = null)
    {
        IQueryable<T> query = Include(includes);
        query = query.Where(r => r.IsActive);
        return predicate != null ? query.Where(predicate).Distinct().ToListAsync() : query.Distinct().ToListAsync();
    }

    public void Add(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(T));
        }

        entity.IsActive = true;
        _entities.Add(entity);
    }

    public void Update(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(T));
        }

        _entities.Update(entity);
    }

    public void SoftDelete(T entity)
    {
        entity.IsActive = false;
        Update(entity);
    }

    public void HardDelete(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(T));
        }

        _entities.Remove(entity);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync() > 0;
    }

    private IQueryable<T> Include(Expression<Func<T, object>>[]? includes = null)
    {
        IQueryable<T> query = _query;
        if (includes != null && includes.Length > 0)
        {
            foreach (var include in includes)
                query = query.Include(include);
        }

        return query;
    }
}
