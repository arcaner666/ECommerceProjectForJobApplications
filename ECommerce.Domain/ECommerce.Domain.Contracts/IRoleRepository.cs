using ECommerce.Domain.Entities.DatabaseModels;

namespace ECommerce.Domain.Contracts;

public interface IRoleRepository : IRepositoryBase<Role>
{
    Task<IQueryable<Role>> GetByIdsAsync(List<short> ids);
}
