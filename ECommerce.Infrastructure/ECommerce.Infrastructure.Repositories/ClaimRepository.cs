using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.DatabaseModels;

namespace ECommerce.Infrastructure.Repositories;

public class ClaimRepository : RepositoryBase<Claim>, IClaimRepository
{
    public ClaimRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
    {

    }

    public async Task<IQueryable<Claim>> GetByIdsAsync(List<int> ids)
    {
        using var connection = RepositoryContext.CreateProjectConnection();
        var claims = await GetByConditionAsync(x => ids.Contains(x.ClaimId));
        return claims.AsQueryable();
    }
}