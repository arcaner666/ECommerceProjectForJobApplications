using Dapper;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.DatabaseModels;
using ECommerce.Domain.Entities.ExtendedDatabaseModels;

namespace ECommerce.Infrastructure.Repositories;

public class RoleClaimRepository : RepositoryBase<RoleClaim>, IRoleClaimRepository
{
    public RoleClaimRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
    {

    }

    public async Task<IQueryable<RoleClaim>> GetByIdsAsync(List<long> ids)
    {
        using var connection = RepositoryContext.CreateProjectConnection();
        var sql = "SELECT"
                  + " role_claim_id AS RoleClaimId,"
                  + " role_id AS RoleId,"
                  + " claim_id AS ClaimId"
                  + " FROM role_claim"
                  + " WHERE role_claim_id = ANY(@ids);";
        var roleClaims = await connection.QueryAsync<RoleClaim>(sql, new { ids });
        return roleClaims.AsQueryable();
    }

    public async Task<IQueryable<RoleClaimExt>> GetExtsByRoleIdAsync(short roleId)
    {
        using var connection = RepositoryContext.CreateProjectConnection();
        var sql = "SELECT"
                  + " rc.role_claim_id AS RoleClaimId,"
                  + " rc.role_id AS RoleId,"
                  + " rc.claim_id AS ClaimId,"
                  + " r.title AS RoleTitle,"
                  + " c.title AS ClaimTitle"
                  + " FROM role_claim rc"
                  + " INNER JOIN role r ON r.role_id = rc.role_id"
                  + " INNER JOIN claim c ON rc.claim_id = c.claim_id"
                  + " WHERE rc.role_id = @roleId";
        var roleClaimExts = await connection.QueryAsync<RoleClaimExt>(sql, new { roleId });
        return roleClaimExts.AsQueryable();
    }
}
