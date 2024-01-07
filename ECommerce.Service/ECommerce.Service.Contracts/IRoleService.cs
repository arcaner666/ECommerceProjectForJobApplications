using ECommerce.Domain.Entities.DTOs.Role;
using ECommerce.Domain.Entities.FilterModels;
using ECommerce.Domain.Entities.Responses;
using Microsoft.AspNetCore.JsonPatch;

namespace ECommerce.Service.Contracts;

public interface IRoleService
{
    Task<IResponse> CreateAsync(RoleForCreateDto entity);
    Task<IResponse> CreateCollectionAsync(List<RoleForCreateDto> entities);
    Task<IResponse> DeleteAsync(short id);
    Task<IResponse> GetListByFilterAsync(RoleFilter? filter);
    Task<IResponse> GetByIdAsync(short id);
    Task<IResponse> GetByIdsAsync(List<short> ids);
    Task<IResponse> GetSingleByFilterAsync(RoleFilter filter);
    Task<IResponse> UpdateAsync(short id, RoleForUpdateDto entity);
    Task<IResponse> UpdatePartiallyAsync(
        short id, JsonPatchDocument<RoleForUpdateDto> jsonPatchDocument);
}
