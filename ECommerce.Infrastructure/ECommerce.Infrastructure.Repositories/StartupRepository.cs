using Dapper;
using ECommerce.Domain.Contracts;
using Microsoft.Extensions.Configuration;

namespace ECommerce.Infrastructure.Repositories;

public class StartupRepository : IStartupRepository
{
    protected readonly RepositoryContext RepositoryContext;
    private readonly IConfiguration _configuration;

    public StartupRepository(
        RepositoryContext repositoryContext,
        IConfiguration configuration
    )
    {
        _configuration = configuration;
        RepositoryContext = repositoryContext;
    }

    public async Task DropAndCreateDatabase()
    {
        using var connection = RepositoryContext.CreateDefaultConnection();
        var os = _configuration.GetSection("ServerConfiguration")["OS"];
        var databaseName = _configuration.GetSection("ConnectionStrings")["DatabaseName"];
        var sql = os switch
        {
            "linux" => $"DROP DATABASE IF EXISTS {databaseName};"
                       + $"CREATE DATABASE {databaseName}"
                       + " TEMPLATE='template0'"
                       + " LC_COLLATE='tr_TR.UTF-8'"
                       + " LC_CTYPE='tr_TR.UTF-8';",

            "windows" => $"DROP DATABASE IF EXISTS {databaseName};"
                         + $"CREATE DATABASE {databaseName};",

            _ => ""
        };

        await connection.ExecuteAsync(sql);
    }

    public async Task DropTables()
    {
        // Sıralama önemli!!!
        using var connection = RepositoryContext.CreateProjectConnection();
        const string sql = "DROP TABLE IF EXISTS person;"
                           + "DROP TABLE IF EXISTS role_claim;"
                           + "DROP TABLE IF EXISTS role;"
                           + "DROP TABLE IF EXISTS claim;";
        await connection.ExecuteAsync(sql);
    }

    public async Task CreateTableClaim()
    {
        using var connection = RepositoryContext.CreateProjectConnection();
        const string sql = "CREATE TABLE claim("
                           + "claim_id  SERIAL4       PRIMARY KEY,"
                           + "title     VARCHAR(500)  NOT NULL);";
        await connection.ExecuteAsync(sql);
    }

    public async Task CreateTableRole()
    {
        using var connection = RepositoryContext.CreateProjectConnection();
        const string sql = "CREATE TABLE role("
                           + "role_id  SERIAL2      PRIMARY KEY,"
                           + "title    VARCHAR(50)  NOT NULL,"
                           + "detail   TEXT         NOT NULL);";
        await connection.ExecuteAsync(sql);
    }

    public async Task CreateTableRoleClaim()
    {
        using var connection = RepositoryContext.CreateProjectConnection();
        const string sql = "CREATE TABLE role_claim("
                           + "role_claim_id  SERIAL8  PRIMARY KEY,"
                           + "role_id        INT2     NOT NULL,"
                           + "claim_id       INT4     NOT NULL,"
                           + "CONSTRAINT fk_role_claim__role FOREIGN KEY (role_id) REFERENCES role (role_id),"
                           + "CONSTRAINT fk_role_claim__claim FOREIGN KEY (claim_id) REFERENCES claim (claim_id));";
        await connection.ExecuteAsync(sql);
    }

    public async Task CreateTablePerson()
    {
        using var connection = RepositoryContext.CreateProjectConnection();
        const string sql = "CREATE TABLE person("
                           + "person_id                  SERIAL8       PRIMARY KEY,"
                           + "role_id                    INT2          NOT NULL,"
                           + "first_name                 VARCHAR(100)  NOT NULL,"
                           + "last_name                  VARCHAR(100)  NOT NULL,"
                           + "email                      VARCHAR(100)  NOT NULL,"
                           + "calling_code               VARCHAR(25)   NOT NULL,"
                           + "phone                      VARCHAR(50)   NOT NULL,"
                           + "password_hash              BYTEA         NOT NULL,"
                           + "password_salt              BYTEA         NOT NULL,"
                           + "refresh_token              VARCHAR(500),"
                           + "refresh_token_expiry_time  TIMESTAMPTZ,"
                           + "created_at                 TIMESTAMPTZ   NOT NULL,"
                           + "updated_at                 TIMESTAMPTZ   NOT NULL,"
                           + "CONSTRAINT fk_person__role FOREIGN KEY (role_id) REFERENCES role (role_id));";
        await connection.ExecuteAsync(sql);
    }
}