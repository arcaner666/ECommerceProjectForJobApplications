using AutoMapper;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.Responses;
using ECommerce.Service.Contracts;

namespace ECommerce.Service.Services;

public class PersonCustomService : IPersonCustomService
{
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly IPersonService _personService;

    public PersonCustomService(
        ILoggerManager logger,
        IMapper mapper,
        IPersonService personService
    )
    {
        _logger = logger;
        _mapper = mapper;
        _personService = personService;
    }

    public async Task<IResponse> InsertCustomPersons()
    {
        //    var personExtForCreateDto = new PersonExtForCreateDto
        //    {
        //        FirstName = "Admin",
        //        LastName = "Admin",
        //        Email = "admin@invoicemanagement.com",
        //        CallingCode = "+90",
        //        Phone = "5554443322",
        //        Password = "Abcde123.",
        //    };
        //    var createAdminResponse = await _personService.CreateAdminAsync(personExtForCreateDto);
        //    if (!createAdminResponse.Success)
        //        return createAdminResponse;

        //    var personExtForCreateDto1 = new PersonExtForCreateDto
        //    {
        //        FirstName = "User1",
        //        LastName = "User1",
        //        Email = "user1@mail.com",
        //        CallingCode = "+90",
        //        Phone = "5552220001",
        //        Password = "Abcde123.",
        //    };
        //    var createUserResponse1 = await _personService.CreateUserAsync(personExtForCreateDto1);
        //    if (!createUserResponse1.Success)
        //        return createUserResponse1;

        //    var personExtForCreateDto2 = new PersonExtForCreateDto
        //    {
        //        FirstName = "User2",
        //        LastName = "User2",
        //        Email = "user2@mail.com",
        //        CallingCode = "+90",
        //        Phone = "5552220002",
        //        Password = "Abcde123.",
        //    };
        //    var createUserResponse2 = await _personService.CreateUserAsync(personExtForCreateDto2);
        //    if (!createUserResponse2.Success)
        //        return createUserResponse2;

        return new SuccessResponse();
    }
}