using ECommerce.Domain.Entities.DTOs.Claim;
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
public class ClaimsController : ControllerBase
{
    private readonly IClaimService _service;

    public ClaimsController(
        IClaimService service
    )
    {
        _service = service;
    }

    [HttpPost]
    [AuthorizationFilter]
    [ValidationFilter(typeof(ClaimForManipulationDtoValidator))]
    [TransactionScopeFilter]
    public async Task<IActionResult> Create(
        [FromBody] ClaimForManipulationDto claimForManipulationDto)
    {
        var response = await _service.CreateAsync(claimForManipulationDto);
        return !response.Success
            ? CustomErrorProcessor.Process(response)
            : CreatedAtRoute(
                "GetClaimById",
                new { id = response.Convert<ClaimDto>().Data.ClaimId },
                response.Convert<ClaimDto>().Data);
    }

    [HttpPost("collection")]
    [AuthorizationFilter]
    [ValidationFilter(typeof(ClaimForManipulationDtoValidator))]
    [TransactionScopeFilter]
    public async Task<IActionResult> CreateCollection(
        [FromBody] List<ClaimForManipulationDto> claimForManipulationDtos)
    {
        var response = await _service.CreateCollectionAsync(claimForManipulationDtos);
        return !response.Success
            ? CustomErrorProcessor.Process(response)
            : CreatedAtRoute(
                "GetClaimsByIds",
                new { ids = response.Convert<ClaimCollectionDto>().Data.Ids },
                response.Convert<ClaimCollectionDto>().Data.ClaimDtos);
    }

    [HttpDelete("{id}")]
    [AuthorizationFilter]
    [TransactionScopeFilter]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _service.DeleteAsync(id);
        return !response.Success
            ? CustomErrorProcessor.Process(response)
            : NoContent();
    }

    [HttpGet("{id}", Name = "GetClaimById")]
    [AuthorizationFilter]
    public async Task<IActionResult> GetById(int id)
    {
        var response = await _service.GetByIdAsync(id);
        return !response.Success
            ? CustomErrorProcessor.Process(response)
            : Ok(response.Convert<ClaimDto>().Data);
    }

    [HttpGet("collection/({ids})", Name = "GetClaimsByIds")]
    [AuthorizationFilter]
    public async Task<IActionResult> GetByIds(
        [ModelBinder(BinderType = typeof(ListModelBinder))] List<int> ids)
    {
        var response = await _service.GetByIdsAsync(ids);
        return !response.Success
            ? CustomErrorProcessor.Process(response)
            : Ok(response.Convert<List<ClaimDto>>().Data);
    }

    [HttpGet]
    //[AuthorizationFilter]
    public async Task<IActionResult> GetListByFilter([FromQuery] ClaimFilter? filter)
    {
        var response = await _service.GetListByFilterAsync(filter);
        return !response.Success
            ? CustomErrorProcessor.Process(response)
            : Ok(response.Convert<List<ClaimDto>>().Data);
    }

    [HttpGet("single")]
    //[AuthorizationFilter]
    public async Task<IActionResult> GetSingleByFilter([FromQuery] ClaimFilter filter)
    {
        var response = await _service.GetSingleByFilterAsync(filter);
        return !response.Success
            ? CustomErrorProcessor.Process(response)
            : Ok(response.Convert<ClaimDto>().Data);
    }

    [HttpPut("{id}")]
    [AuthorizationFilter]
    [ValidationFilter(typeof(ClaimForManipulationDtoValidator))]
    [TransactionScopeFilter]
    public async Task<IActionResult> Update(
        int id, [FromBody] ClaimForManipulationDto claimForManipulationDto)
    {
        var response = await _service.UpdateAsync(id, claimForManipulationDto);
        return !response.Success
            ? CustomErrorProcessor.Process(response)
            : NoContent();
    }

    [HttpPatch("{id}")]
    [AuthorizationFilter]
    [TransactionScopeFilter]
    public async Task<IActionResult> UpdatePartially(
        int id, [FromBody] JsonPatchDocument<ClaimForManipulationDto> claimForManipulationDtoPatchDoc)
    {
        var response = await _service.UpdatePartiallyAsync(id, claimForManipulationDtoPatchDoc);
        return !response.Success
            ? CustomErrorProcessor.Process(response)
            : NoContent();
    }
}