using ECommerce.Domain.Entities.Responses;

namespace ECommerce.Service.Contracts;

public interface IPersonCustomService
{
    Task<IResponse> InsertCustomPersons();
}
