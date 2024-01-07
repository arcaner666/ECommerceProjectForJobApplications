using ECommerce.Domain.Entities.Responses;

namespace ECommerce.Service.Contracts;

public interface IClaimDefaultService
{
    Task<IResponse> InsertDefaultClaims();
}
