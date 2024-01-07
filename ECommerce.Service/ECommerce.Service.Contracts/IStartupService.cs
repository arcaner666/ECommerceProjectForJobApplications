using ECommerce.Domain.Entities.Responses;

namespace ECommerce.Service.Contracts;

public interface IStartupService
{
    Task<IResponse> ApplyDatabaseChanges();
    Task<IResponse> DropAndCreateDatabase();
    Task<IResponse> DropTables();
    Task<IResponse> CreateTables();
    Task<IResponse> InsertDefaultRecords();
    Task<IResponse> InsertCustomRecords();
}
