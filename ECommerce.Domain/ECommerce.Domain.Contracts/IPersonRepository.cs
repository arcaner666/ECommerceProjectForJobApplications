using ECommerce.Domain.Entities.DatabaseModels;
using ECommerce.Domain.Entities.ExtendedDatabaseModels;

namespace ECommerce.Domain.Contracts;

public interface IPersonRepository : IRepositoryBase<Person>
{
    Task<IQueryable<Person>> GetByIdsAsync(List<long> ids);
    Task<PersonExt> GetExtByIdAsync(long id);
    Task<IQueryable<PersonExt>> GetExtsAsync();
}
