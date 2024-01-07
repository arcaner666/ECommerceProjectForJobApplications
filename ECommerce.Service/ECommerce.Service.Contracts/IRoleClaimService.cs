using ECommerce.Domain.Entities.DTOs.RoleClaim;
using ECommerce.Domain.Entities.FilterModels;
using ECommerce.Domain.Entities.Responses;

namespace ECommerce.Service.Contracts;

public interface IRoleClaimService
{
    Task<IResponse> CreateAsync(RoleClaimForCreateDto roleClaimForCreateDto);
    Task<IResponse> CreateCollectionAsync(List<RoleClaimForCreateDto> roleClaimForCreateDtos);
    Task<IResponse> DeleteAsync(long id);
    Task<IResponse> GetByIdAsync(long id);
    Task<IResponse> GetByIdsAsync(List<long> ids);
    Task<IResponse> GetExtsByRoleIdAsync(short roleId);
    Task<IResponse> GetListByFilterAsync(RoleClaimFilter? filter);
    Task<IResponse> GetSingleByFilterAsync(RoleClaimFilter filter);
}
