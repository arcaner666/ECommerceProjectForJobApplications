using Dapper;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.DatabaseModels;
using ECommerce.Domain.Entities.ExtendedDatabaseModels;

namespace ECommerce.Infrastructure.Repositories;

public class PersonRepository : RepositoryBase<Person>, IPersonRepository
{
    public PersonRepository(RepositoryContext repositoryContext)
        : base(repositoryContext)
    {

    }

    public async Task<IQueryable<Person>> GetByIdsAsync(List<long> ids)
    {
        using var connection = RepositoryContext.CreateProjectConnection();
        const string sql = "SELECT"
                  + " person_id AS PersonId,"
                  + " role_id AS RoleId,"
                  + " first_name AS FirstName,"
                  + " last_name AS LastName,"
                  + " email AS Email,"
                  + " calling_code AS CallingCode,"
                  + " phone AS Phone,"
                  + " refresh_token AS RefreshToken,"
                  + " refresh_token_expiry_time AS RefreshTokenExpiryTime,"
                  + " created_at AS CreatedAt,"
                  + " updated_at AS UpdatedAt"
                  + " FROM person"
                  + " WHERE person_id = ANY(@Ids);";
        var persons = await connection.QueryAsync<Person>(sql, new { @Ids = ids });
        return persons.AsQueryable();
    }

    public async Task<PersonExt> GetExtByIdAsync(long id)
    {
        using var connection = RepositoryContext.CreateProjectConnection();
        const string sql = "SELECT"
                  + " p.person_id AS PersonId,"
                  + " p.role_id AS RoleId,"
                  + " p.first_name AS FirstName,"
                  + " p.last_name AS LastName,"
                  + " p.email AS Email,"
                  + " p.calling_code AS CallingCode,"
                  + " p.phone AS Phone,"
                  + " p.refresh_token AS RefreshToken,"
                  + " p.refresh_token_expiry_time AS RefreshTokenExpiryTime,"
                  + " p.created_at AS CreatedAt,"
                  + " p.updated_at AS UpdatedAt,"
                  + " r.title AS RoleTitle"
                  + " FROM person p"
                  + " INNER JOIN role r ON r.role_id = p.role_id"
                  + " WHERE p.person_id = @id;";
        var personExt = await connection.QueryAsync<PersonExt>(sql, new { id });
        return personExt.SingleOrDefault();
    }

    public async Task<IQueryable<PersonExt>> GetExtsAsync()
    {
        using var connection = RepositoryContext.CreateProjectConnection();
        const string sql = "SELECT"
                  + " p.person_id AS PersonId,"
                  + " p.role_id AS RoleId,"
                  + " p.first_name AS FirstName,"
                  + " p.last_name AS LastName,"
                  + " p.email AS Email,"
                  + " p.calling_code AS CallingCode,"
                  + " p.phone AS Phone,"
                  + " p.refresh_token AS RefreshToken,"
                  + " p.refresh_token_expiry_time AS RefreshTokenExpiryTime,"
                  + " p.created_at AS CreatedAt,"
                  + " p.updated_at AS UpdatedAt,"
                  + " r.title AS RoleTitle"
                  + " FROM person p"
                  + " INNER JOIN role r ON r.role_id = p.role_id;";
        var personExts = await connection.QueryAsync<PersonExt>(sql);
        return personExts.AsQueryable();
    }
}