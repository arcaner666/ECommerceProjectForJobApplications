using System.Linq.Expressions;

namespace ECommerce.Domain.Contracts;

public interface IRepositoryBase<T>
{
    Task<T> CreateAsync(T entity);
    Task<bool> DeleteAsync(T entity);
    Task<IQueryable<T>> GetAllAsync();
    Task<IQueryable<T>> GetByConditionAsync(Expression<Func<T, bool>> expression);
    Task<T?> GetByIdAsync(T entity);
    Task<T> UpdateAsync(T entity);
}