using ECommerce.Domain.Entities.Responses;

namespace ECommerce.Service.Contracts;

public interface IRoleDefaultService
{
    Task<IResponse> InsertDefaultRoles();
}
