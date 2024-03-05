using ECommerce.Domain.Entities.DTOs.Person;
using ECommerce.Domain.Entities.Responses.Extensions;
using ECommerce.Presentation.Presentation.ActionFilters;
using ECommerce.Presentation.Presentation.Extensions;
using ECommerce.Presentation.Presentation.ModelBinders;
using ECommerce.Presentation.Presentation.Validation.FluentValidation.Validators;
using ECommerce.Service.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Presentation.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PersonsController : ControllerBase
{
    private readonly IPersonService _service;

    public PersonsController(
        IPersonService service
    )
    {
        _service = service;
    }

    //[HttpPost("createadmin")]
    //[AuthorizationFilter]
    //[ValidationFilter(typeof(PersonExtForCreateDtoValidator))]
    //[TransactionScopeFilter]
    //public async Task<IActionResult> CreateAdmin([FromBody] PersonExtForCreateDto personExtForCreateDto)
    //{
    //    var response = await _service.CreateAdminAsync(personExtForCreateDto);
    //    return !response.Success
    //        ? CustomErrorProcessor.Process(response)
    //        : CreatedAtRoute(
    //            "GetPersonById",
    //            new { id = response.Convert<PersonDto>().Data.PersonId },
    //            response.Convert<PersonDto>().Data);
    //}

    //[HttpPost("createuser")]
    //[ValidationFilter(typeof(PersonExtForCreateDtoValidator))]
    //[TransactionScopeFilter]
    //public async Task<IActionResult> CreateUser([FromBody] PersonExtForCreateDto personExtForCreateDto)
    //{
    //    var response = await _service.CreateUserAsync(personExtForCreateDto);
    //    return !response.Success
    //        ? CustomErrorProcessor.Process(response)
    //        : CreatedAtRoute(
    //            "GetPersonById",
    //            new { id = response.Convert<PersonDto>().Data.PersonId },
    //            response.Convert<PersonDto>().Data);
    //}

    [HttpGet("{id}", Name = "GetPersonById")]
    [AuthorizationFilter]
    public async Task<IActionResult> GetById(long id)
    {
        var response = await _service.GetByIdAsync(id);
        return !response.Success ? CustomErrorProcessor.Process(response) : Ok(response.Convert<PersonDto>().Data);
    }

    [HttpGet("collection/({ids})", Name = "GetPersonsByIds")]
    [AuthorizationFilter]
    public async Task<IActionResult> GetByIds([ModelBinder(BinderType = typeof(ListModelBinder))] List<long> ids)
    {
        var response = await _service.GetByIdsAsync(ids);
        return !response.Success ? CustomErrorProcessor.Process(response) : Ok(response.Convert<List<PersonDto>>().Data);
    }

    [HttpGet("getextbyid/{id}")]
    [AuthorizationFilter]
    public async Task<IActionResult> GetExtById(long id)
    {
        var response = await _service.GetExtByIdAsync(id);
        return !response.Success ? CustomErrorProcessor.Process(response) : Ok(response.Convert<PersonExtDto>().Data);
    }

    [HttpGet("getexts")]
    [AuthorizationFilter]
    public async Task<IActionResult> GetExts()
    {
        var response = await _service.GetExtsAsync();
        return !response.Success ? CustomErrorProcessor.Process(response) : Ok(response.Convert<List<PersonExtDto>>().Data);
    }

    //[HttpPost("login")]
    //[ValidationFilter(typeof(PersonLoginDtoValidator))]
    //[TransactionScopeFilter]
    //public async Task<IActionResult> Login([FromBody] PersonLoginDto personLoginDto)
    //{
    //    var response = await _service.LoginAsync(personLoginDto);
    //    return !response.Success ? CustomErrorProcessor.Process(response) : Ok(response.Convert<PersonLoginResponseDto>().Data);
    //}

    //[HttpGet("logout")]
    //[TransactionScopeFilter]
    //public async Task<IActionResult> Logout()
    //{
    //    var response =
    //        await _service.LogoutAsync(HttpContext.User.PersonId());
    //    return !response.Success ? CustomErrorProcessor.Process(response) : NoContent();
    //}

    //[HttpPost("refreshaccesstoken")]
    //[ValidationFilter(typeof(PersonTokenDtoValidator))]
    //public async Task<IActionResult> RefreshAccessToken([FromBody] PersonTokenDto personTokenDto)
    //{
    //    var response = await _service.RefreshAccessTokenAsync(personTokenDto);
    //    return !response.Success ? CustomErrorProcessor.Process(response) : Ok(response.Convert<PersonTokenDto>().Data);
    //}

    [HttpPut("{id}")]
    [AuthorizationFilter]
    [ValidationFilter(typeof(PersonForUpdateDtoValidator))]
    [TransactionScopeFilter]
    public async Task<IActionResult> Update(long id,
        [FromBody] PersonForUpdateDto personForUpdateDto)
    {
        var response = await _service.UpdateAsync(id, personForUpdateDto);
        return !response.Success ? CustomErrorProcessor.Process(response) : NoContent();
    }
}