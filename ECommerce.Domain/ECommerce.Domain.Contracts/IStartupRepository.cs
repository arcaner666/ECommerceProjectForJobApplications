namespace ECommerce.Domain.Contracts;

public interface IStartupRepository
{
    Task DropAndCreateDatabase();
    Task DropTables();
    Task CreateTableClaim();
    Task CreateTableRole();
    Task CreateTableRoleClaim();
    Task CreateTablePerson();
}