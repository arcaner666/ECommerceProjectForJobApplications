using ECommerce.Domain.Entities.DTOs.Person;
using ECommerce.Domain.Entities.Responses;

namespace ECommerce.Service.Contracts;

public interface IPersonService
{
    Task<IResponse> CreateAsync(PersonForCreateDto personForCreateDto);
    Task<IResponse> CreateCollectionAsync(List<PersonForCreateDto> personForCreateDtos);
    Task<IResponse> DeleteAsync(long id);
    Task<IResponse> GetAllAsync();
    Task<IResponse> GetByEmailAsync(string email);
    Task<IResponse> GetByIdAsync(long id);
    Task<IResponse> GetByIdsAsync(List<long> ids);
    Task<IResponse> GetExtByIdAsync(long id);
    Task<IResponse> GetExtsAsync();
    Task<IResponse> UpdateAfterLoginAsync(long id, PersonForUpdateAfterLoginDto personForUpdateAfterLoginDto);
    Task<IResponse> UpdateAsync(long id, PersonForUpdateDto personForUpdateDto);
}
