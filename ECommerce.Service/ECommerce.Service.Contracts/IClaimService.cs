using ECommerce.Domain.Entities.DTOs.Claim;
using ECommerce.Domain.Entities.FilterModels;
using ECommerce.Domain.Entities.Responses;
using Microsoft.AspNetCore.JsonPatch;

namespace ECommerce.Service.Contracts;

public interface IClaimService
{
    Task<IResponse> CreateAsync(ClaimForManipulationDto entity);
    Task<IResponse> CreateCollectionAsync(List<ClaimForManipulationDto> entities);
    Task<IResponse> DeleteAsync(int id);
    Task<IResponse> GetByIdAsync(int id);
    Task<IResponse> GetByIdsAsync(List<int> ids);
    Task<IResponse> GetListByFilterAsync(ClaimFilter? filter);
    Task<IResponse> GetSingleByFilterAsync(ClaimFilter filter);
    Task<IResponse> UpdateAsync(int id, ClaimForManipulationDto entity);
    Task<IResponse> UpdatePartiallyAsync(
        int id, JsonPatchDocument<ClaimForManipulationDto> jsonPatchDocument);
}
