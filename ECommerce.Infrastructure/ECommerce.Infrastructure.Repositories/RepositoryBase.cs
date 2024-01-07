using Dapper.FastCrud;
using ECommerce.Domain.Contracts;
using System.Linq.Expressions;

namespace ECommerce.Infrastructure.Repositories;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected readonly RepositoryContext RepositoryContext;

    protected RepositoryBase(RepositoryContext repositoryContext)
    {
        RepositoryContext = repositoryContext;
    }

    public async Task<T> CreateAsync(T entity)
    {
        using var connection = RepositoryContext.CreateProjectConnection();
        await connection.InsertAsync(entity);
        return entity;
    }

    public async Task<bool> DeleteAsync(T entity)
    {
        using var connection = RepositoryContext.CreateProjectConnection();
        return await connection.DeleteAsync(entity);
    }

    public async Task<IQueryable<T>> GetAllAsync()
    {
        using var connection = RepositoryContext.CreateProjectConnection();
        var entities = await connection.FindAsync<T>();
        return entities.AsQueryable();
    }

    public async Task<IQueryable<T>> GetByConditionAsync(Expression<Func<T, bool>> expression)
    {
        using var connection = RepositoryContext.CreateProjectConnection();
        var entities = await connection.FindAsync<T>();
        return entities.AsQueryable().Where(expression);
    }

    public async Task<T?> GetByIdAsync(T entity)
    {
        using var connection = RepositoryContext.CreateProjectConnection();
        var searchedEntity = await connection.GetAsync<T>(entity);
        return searchedEntity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        using var connection = RepositoryContext.CreateProjectConnection();
        await connection.UpdateAsync(entity);
        return entity;
    }
}