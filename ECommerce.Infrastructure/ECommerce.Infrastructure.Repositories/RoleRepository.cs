using Dapper;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.DatabaseModels;

namespace ECommerce.Infrastructure.Repositories;

public class RoleRepository : RepositoryBase<Role>, IRoleRepository
{
    public RoleRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
    {

    }

    public async Task<IQueryable<Role>> GetByIdsAsync(List<short> ids)
    {
        using var connection = RepositoryContext.CreateProjectConnection();
        var sql = "SELECT"
                  + " role_id AS RoleId,"
                  + " title AS Title,"
                  + " detail AS Detail"
                  + " FROM role"
                  + " WHERE role_id = ANY(@Ids);";
        var roles = await connection.QueryAsync<Role>(sql, new { @Ids = ids });
        return roles.AsQueryable();
    }
}
