using ECommerce.Domain.Entities.DTOs.Person;
using ECommerce.Domain.Entities.Responses;

namespace ECommerce.Service.Contracts;

public interface IPersonAdvService
{
    Task<IResponse> CreateAdminAsync(PersonExtForCreateDto personExtForCreateDto);
    Task<IResponse> CreateUserAsync(PersonExtForCreateDto personExtForCreateDto);
    Task<IResponse> CreateUserCollectionAsync(List<PersonExtForCreateDto> personExtForCreateDtos);
    Task<IResponse> LoginAsync(PersonLoginDto personLoginDto);
    Task<IResponse> LogoutAsync(long id);
    Task<IResponse> RefreshAccessTokenAsync(PersonTokenDto personTokenDto);
}
