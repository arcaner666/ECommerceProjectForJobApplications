using ECommerce.Domain.Entities.DatabaseModels;
using ECommerce.Domain.Entities.ExtendedDatabaseModels;

namespace ECommerce.Domain.Contracts;

public interface IRoleClaimRepository : IRepositoryBase<RoleClaim>
{
    Task<IQueryable<RoleClaim>> GetByIdsAsync(List<long> ids);
    Task<IQueryable<RoleClaimExt>> GetExtsByRoleIdAsync(short roleId);
}
