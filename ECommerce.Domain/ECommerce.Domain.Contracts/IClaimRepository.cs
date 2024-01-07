using ECommerce.Domain.Entities.DatabaseModels;

namespace ECommerce.Domain.Contracts;

public interface IClaimRepository : IRepositoryBase<Claim>
{
    Task<IQueryable<Claim>> GetByIdsAsync(List<int> ids);
}
