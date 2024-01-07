using AutoMapper;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.DTOs.Person;
using ECommerce.Domain.Entities.Responses;
using ECommerce.Service.Contracts;

namespace ECommerce.Service.Services;

public class PersonCustomService : IPersonCustomService
{
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly IPersonAdvService _personAdvService;

    public PersonCustomService(
        ILoggerManager logger,
        IMapper mapper,
        IPersonAdvService personAdvService
    )
    {
        _logger = logger;
        _mapper = mapper;
        _personAdvService = personAdvService;
    }

    public async Task<IResponse> InsertCustomPersons()
    {
        var personExtForCreateDto = new PersonExtForCreateDto
        {
            FirstName = "Admin",
            LastName = "Admin",
            Email = "admin@invoicemanagement.com",
            CallingCode = "+90",
            Phone = "5554443322",
            Password = "Abcde123.",
        };
        var createAdminResponse = await _personAdvService.CreateAdminAsync(personExtForCreateDto);
        if (!createAdminResponse.Success)
            return createAdminResponse;

        var personExtForCreateDtos = new List<PersonExtForCreateDto>
         {
             new()
             {
                 FirstName = "User1",
                 LastName = "User1",
                 Email = "user1@mail.com",
                 CallingCode = "+90",
                 Phone = "5552220001",
                 Password = "Abcde123.",
             },
             new()
             {
                 FirstName = "User2",
                 LastName = "User2",
                 Email = "user2@mail.com",
                 CallingCode = "+90",
                 Phone = "5552220002",
                 Password = "Abcde123.",
             },
         };
        var createUserResponse = await _personAdvService.CreateUserCollectionAsync(personExtForCreateDtos);
        if (!createUserResponse.Success)
            return createUserResponse;

        return new SuccessResponse();
    }
}