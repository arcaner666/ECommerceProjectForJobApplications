using ECommerce.Presentation.Presentation.ActionFilters;
using ECommerce.Presentation.Presentation.Extensions;
using ECommerce.Service.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Presentation.Presentation.Controllers;

[Route("[controller]")]
[ApiController]
public class StartupController : ControllerBase
{
    private readonly IStartupService _service;

    public StartupController(
        IStartupService service
    )
    {
        _service = service;
    }

    [HttpGet("apply-database-changes")]
    //[AuthorizationFilter]
    [TransactionScopeFilter]
    public async Task<IActionResult> ApplyDatabaseChanges()
    {
        var response = await _service.ApplyDatabaseChanges();
        return !response.Success
            ? CustomErrorProcessor.Process(response)
            : NoContent();
    }

    [HttpGet("drop-and-create-database")]
    //[AuthorizationFilter]
    public async Task<IActionResult> DropAndCreateDatabase()
    {
        var response = await _service.DropAndCreateDatabase();
        return !response.Success
            ? CustomErrorProcessor.Process(response)
            : NoContent();
    }

    [HttpGet("createtables")]
    [AuthorizationFilter]
    [TransactionScopeFilter]
    public async Task<IActionResult> CreateTables()
    {
        var response = await _service.CreateTables();
        return !response.Success
            ? CustomErrorProcessor.Process(response)
            : NoContent();
    }

    [HttpGet("droptables")]
    [AuthorizationFilter]
    [TransactionScopeFilter]
    public async Task<IActionResult> DropTables()
    {
        var response = await _service.DropTables();
        return !response.Success
            ? CustomErrorProcessor.Process(response)
            : NoContent();
    }

    [HttpGet("insertcustomrecords")]
    [AuthorizationFilter]
    [TransactionScopeFilter]
    public async Task<IActionResult> InsertCustomRecords()
    {
        var response = await _service.InsertCustomRecords();
        return !response.Success
            ? CustomErrorProcessor.Process(response)
            : NoContent();
    }

    [HttpGet("insertdefaultrecords")]
    [AuthorizationFilter]
    [TransactionScopeFilter]
    public async Task<IActionResult> InsertDefaultRecords()
    {
        var response = await _service.InsertDefaultRecords();
        return !response.Success
            ? CustomErrorProcessor.Process(response)
            : NoContent();
    }
}