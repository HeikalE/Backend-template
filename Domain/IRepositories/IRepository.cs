using Domain.Entities;
using System.Linq.Expressions;

namespace Domain.IRepositories;

public interface IRepository<T> where T : Base
{
    Task<T?> GetAsync(Expression<Func<T, bool>> predicate, Expression<Func<T, object>>[]? includes = null);
    Task<List<T>> GetListAsync(Expression<Func<T, bool>>? predicate = null, Expression<Func<T, object>>[]? includes = null);
    Task<List<T>> GetDistinctListAsync(Expression<Func<T, bool>>? predicate = null, Expression<Func<T, object>>[]? includes = null);

    void Add(T entity);
    void Update(T entity);
    void SoftDelete(T entity);
    void HardDelete(T entity);

    public Task<bool> SaveChangesAsync();
}
