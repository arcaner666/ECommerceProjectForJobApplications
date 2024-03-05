using ECommerce.Domain.Entities.DTOs.Person;
using ECommerce.Domain.Entities.FilterModels;
using ECommerce.Domain.Entities.Responses;

namespace ECommerce.Service.Contracts;

public interface IPersonService
{
    Task<IResponse> CreateAsync(PersonForCreateDto entity);
    Task<IResponse> CreateCollectionAsync(List<PersonForCreateDto> entities);
    //Task<IResponse> CreateAdminAsync(PersonExtForCreateDto entity);
    //Task<IResponse> CreateUserAsync(PersonExtForCreateDto entity);
    Task<IResponse> DeleteAsync(long id);
    Task<IResponse> GetByIdAsync(long id);
    Task<IResponse> GetByIdsAsync(List<long> ids);
    Task<IResponse> GetExtByIdAsync(long id);
    Task<IResponse> GetExtsAsync();
    Task<IResponse> GetListByFilterAsync(PersonFilter? filter);
    Task<IResponse> GetSingleByFilterAsync(PersonFilter filter);
    //Task<IResponse> LoginAsync(PersonLoginDto entity);
    //Task<IResponse> LogoutAsync(long id);
    //Task<IResponse> RefreshAccessTokenAsync(PersonTokenDto entity);
    Task<IResponse> UpdateAfterLoginAsync(long id, PersonForUpdateAfterLoginDto entity);
    Task<IResponse> UpdateAsync(long id, PersonForUpdateDto entity);
}
