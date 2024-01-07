using ECommerce.Domain.Entities.DTOs.RoleClaim;
using ECommerce.Domain.Entities.Responses;

namespace ECommerce.Service.Contracts;

public interface ITokenHelper
{
    string GenerateAccessToken(long personId, string role, List<RoleClaimExtDto> roleClaimExtDtos);
    string GenerateRefreshToken();
    IResponse GetPrincipalFromExpiredToken(string accessToken);
}
