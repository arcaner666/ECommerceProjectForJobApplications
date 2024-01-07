using Dapper.FastCrud;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace ECommerce.Infrastructure.Repositories;

public class RepositoryContext
{
    private readonly string _defaultConnectionString;
    private readonly string _projectConnectionString;

    public RepositoryContext(IConfiguration configuration)
    {
        _defaultConnectionString = configuration.GetConnectionString("DefaultConnectionString");
        _projectConnectionString = configuration.GetConnectionString("ProjectConnectionString");
        OrmConfiguration.DefaultDialect = SqlDialect.PostgreSql;
    }

    public virtual IDbConnection CreateDefaultConnection()
    {
        return new NpgsqlConnection(_defaultConnectionString);
    }

    public virtual IDbConnection CreateProjectConnection()
    {
        return new NpgsqlConnection(_projectConnectionString);
    }
}
