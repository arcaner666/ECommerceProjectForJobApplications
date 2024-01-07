using ECommerce.Domain.Entities.DTOs.RoleClaim;
using ECommerce.Domain.Entities.FilterModels;
using ECommerce.Domain.Entities.Responses.Extensions;
using ECommerce.Presentation.Presentation.ActionFilters;
using ECommerce.Presentation.Presentation.Extensions;
using ECommerce.Presentation.Presentation.ModelBinders;
using ECommerce.Presentation.Presentation.Validation.FluentValidation.Validators;
using ECommerce.Service.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Presentation.Presentation.Controllers;

[Route("[controller]")]
[ApiController]
public class RoleClaimsController : ControllerBase
{
    private readonly IRoleClaimService _service;

    public RoleClaimsController(
        IRoleClaimService service
    )
    {
        _service = service;
    }

    [HttpPost]
    [AuthorizationFilter]
    [ValidationFilter(typeof(RoleClaimForCreateDtoValidator))]
    [TransactionScopeFilter]
    public async Task<IActionResult> Create(
        [FromBody] RoleClaimForCreateDto roleClaimForCreateDto)
    {
        var response = await _service.CreateAsync(roleClaimForCreateDto);
        return !response.Success
            ? CustomErrorProcessor.Process(response)
            : CreatedAtRoute(
                "GetRoleClaimById",
                new { id = response.Convert<RoleClaimDto>().Data.RoleClaimId },
                response.Convert<RoleClaimDto>().Data);
    }

    [HttpPost("collection")]
    [AuthorizationFilter]
    [ValidationFilter(typeof(RoleClaimForCreateDtoValidator))]
    [TransactionScopeFilter]
    public async Task<IActionResult> CreateCollection(
        [FromBody] List<RoleClaimForCreateDto> roleClaimForCreateDtos)
    {
        var response = await _service.CreateCollectionAsync(roleClaimForCreateDtos);
        return !response.Success
            ? CustomErrorProcessor.Process(response)
            : CreatedAtRoute(
                "GetRoleClaimsByIds",
                new { ids = response.Convert<RoleClaimCollectionDto>().Data.Ids },
                response.Convert<RoleClaimCollectionDto>().Data.RoleClaimDtos);
    }

    [HttpDelete("{id}")]
    [AuthorizationFilter]
    [TransactionScopeFilter]
    public async Task<IActionResult> Delete(long id)
    {
        var response = await _service.DeleteAsync(id);
        return !response.Success
            ? CustomErrorProcessor.Process(response)
            : NoContent();
    }

    [HttpGet("{id}", Name = "GetRoleClaimById")]
    [AuthorizationFilter]
    public async Task<IActionResult> GetById(long id)
    {
        var response = await _service.GetByIdAsync(id);
        return !response.Success
            ? CustomErrorProcessor.Process(response)
            : Ok(response.Convert<RoleClaimDto>().Data);
    }

    [HttpGet("collection/({ids})", Name = "GetRoleClaimsByIds")]
    [AuthorizationFilter]
    public async Task<IActionResult> GetByIds([ModelBinder(BinderType = typeof(ListModelBinder))] List<long> ids)
    {
        var response = await _service.GetByIdsAsync(ids);
        return !response.Success
            ? CustomErrorProcessor.Process(response)
            : Ok(response.Convert<List<RoleClaimDto>>().Data);
    }

    [HttpGet("getextsbyroleid/{roleId}")]
    [AuthorizationFilter]
    public async Task<IActionResult> GetExtsByRoleId(short roleId)
    {
        var response = await _service.GetExtsByRoleIdAsync(roleId);
        return !response.Success
            ? CustomErrorProcessor.Process(response)
            : Ok(response.Convert<List<RoleClaimExtDto>>().Data);
    }

    [HttpGet]
    [AuthorizationFilter]
    public async Task<IActionResult> GetListByFilter([FromQuery] RoleClaimFilter? filter)
    {
        var response = await _service.GetListByFilterAsync(filter);
        return !response.Success
            ? CustomErrorProcessor.Process(response)
            : Ok(response.Convert<List<RoleClaimDto>>().Data);
    }
}
