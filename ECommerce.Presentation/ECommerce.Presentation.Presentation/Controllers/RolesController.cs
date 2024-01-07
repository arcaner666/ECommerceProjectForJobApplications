using ECommerce.Domain.Entities.DTOs.Role;
using ECommerce.Domain.Entities.FilterModels;
using ECommerce.Domain.Entities.Responses.Extensions;
using ECommerce.Presentation.Presentation.ActionFilters;
using ECommerce.Presentation.Presentation.Extensions;
using ECommerce.Presentation.Presentation.ModelBinders;
using ECommerce.Presentation.Presentation.Validation.FluentValidation.Validators;
using ECommerce.Service.Contracts;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Presentation.Presentation.Controllers;

[Route("[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly IRoleService _service;

    public RolesController(
        IRoleService service
    )
    {
        _service = service;
    }

    [HttpPost]
    [AuthorizationFilter]
    [ValidationFilter(typeof(RoleForCreateDtoValidator))]
    [TransactionScopeFilter]
    public async Task<IActionResult> Create(
        [FromBody] RoleForCreateDto roleForManipulationDto)
    {
        var response = await _service.CreateAsync(roleForManipulationDto);
        return !response.Success
            ? CustomErrorProcessor.Process(response)
            : CreatedAtRoute(
            "GetRoleById",
            new { id = response.Convert<RoleDto>().Data.RoleId },
            response.Convert<RoleDto>().Data);
    }

    [HttpPost("collection")]
    [AuthorizationFilter]
    [ValidationFilter(typeof(RoleForCreateDtoValidator))]
    [TransactionScopeFilter]
    public async Task<IActionResult> CreateCollection(
        [FromBody] List<RoleForCreateDto> roleForManipulationDtos)
    {
        var response = await _service.CreateCollectionAsync(roleForManipulationDtos);
        return !response.Success
            ? CustomErrorProcessor.Process(response)
            : CreatedAtRoute(
                "GetRolesByIds",
                new { ids = response.Convert<RoleCollectionDto>().Data.Ids },
                response.Convert<RoleCollectionDto>().Data.RoleDtos);
    }

    [HttpDelete("{id}")]
    [AuthorizationFilter]
    [TransactionScopeFilter]
    public async Task<IActionResult> Delete(short id)
    {
        var response = await _service.DeleteAsync(id);
        return !response.Success
            ? CustomErrorProcessor.Process(response)
            : NoContent();
    }

    [HttpGet("{id}", Name = "GetRoleById")]
    [AuthorizationFilter]
    public async Task<IActionResult> GetById(short id)
    {
        var response = await _service.GetByIdAsync(id);
        return !response.Success
            ? CustomErrorProcessor.Process(response)
            : Ok(response.Convert<RoleDto>().Data);
    }

    [HttpGet("collection/({ids})", Name = "GetRolesByIds")]
    [AuthorizationFilter]
    public async Task<IActionResult> GetByIds(
        [ModelBinder(BinderType = typeof(ListModelBinder))] List<short> ids)
    {
        var response = await _service.GetByIdsAsync(ids);
        return !response.Success
            ? CustomErrorProcessor.Process(response)
            : Ok(response.Convert<List<RoleDto>>().Data);
    }

    [HttpGet]
    [AuthorizationFilter]
    public async Task<IActionResult> GetListByFilter([FromQuery] RoleFilter? filter)
    {
        var response = await _service.GetListByFilterAsync(filter);
        return !response.Success
            ? CustomErrorProcessor.Process(response)
            : Ok(response.Convert<List<RoleDto>>().Data);
    }

    [HttpGet("single")]
    [AuthorizationFilter]
    public async Task<IActionResult> GetSingleByFilter([FromQuery] RoleFilter filter)
    {
        var response = await _service.GetSingleByFilterAsync(filter);
        return !response.Success
            ? CustomErrorProcessor.Process(response)
            : Ok(response.Convert<RoleDto>().Data);
    }

    [HttpPut("{id}")]
    [AuthorizationFilter]
    [ValidationFilter(typeof(RoleForUpdateDtoValidator))]
    [TransactionScopeFilter]
    public async Task<IActionResult> Update(
        short id, [FromBody] RoleForUpdateDto roleForUpdateDto)
    {
        var response = await _service.UpdateAsync(id, roleForUpdateDto);
        return !response.Success
            ? CustomErrorProcessor.Process(response)
            : NoContent();
    }

    [HttpPatch("{id}")]
    [AuthorizationFilter]
    [TransactionScopeFilter]
    public async Task<IActionResult> UpdatePartially(
        short id, [FromBody] JsonPatchDocument<RoleForUpdateDto> roleForUpdateDtoPatchDoc)
    {
        var response = await _service.UpdatePartiallyAsync(id, roleForUpdateDtoPatchDoc);
        return !response.Success
            ? CustomErrorProcessor.Process(response)
            : NoContent();
    }
}
