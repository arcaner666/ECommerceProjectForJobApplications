using AutoMapper;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.DTOs.Claim;
using ECommerce.Domain.Entities.Responses;
using ECommerce.Service.Contracts;

namespace ECommerce.Service.Services;

public class ClaimDefaultService : IClaimDefaultService
{
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly IClaimService _service;

    public ClaimDefaultService(
        ILoggerManager logger,
        IMapper mapper,
        IClaimService service
    )
    {
        _logger = logger;
        _mapper = mapper;
        _service = service;
    }

    public async Task<IResponse> InsertDefaultClaims()
    {
        var entities = new List<ClaimForManipulationDto>
        {
            new() { Title = "Claims-Create" },
            new() { Title = "Claims-CreateCollection" },
            new() { Title = "Claims-Delete" },
            new() { Title = "Claims-GetListByFilter" },
            new() { Title = "Claims-GetById" },
            new() { Title = "Claims-GetByIds" },
            new() { Title = "Claims-GetSingleByFilter" },
            new() { Title = "Claims-Update" },
            new() { Title = "Claims-UpdatePartially" },
            new() { Title = "Persons-CreateAdmin" },
            new() { Title = "Persons-CreateUser" },
            new() { Title = "Persons-GetAll" },
            new() { Title = "Persons-GetById" },
            new() { Title = "Persons-GetByIds" },
            new() { Title = "Persons-GetExtById" },
            new() { Title = "Persons-GetExts" },
            new() { Title = "Persons-Login" },
            new() { Title = "Persons-Logout" },
            new() { Title = "Persons-RefreshAccessToken" },
            new() { Title = "Persons-Update" },
            new() { Title = "RoleClaims-GetAll" },
            new() { Title = "RoleClaims-GetById" },
            new() { Title = "RoleClaims-GetByIds" },
            new() { Title = "RoleClaims-GetByRoleId" },
            new() { Title = "RoleClaims-GetExtsByRoleId" },
            new() { Title = "RoleClaims-Update" },
            new() { Title = "Roles-Create" },
            new() { Title = "Roles-Delete" },
            new() { Title = "Roles-GetAll" },
            new() { Title = "Roles-GetById" },
            new() { Title = "Roles-GetByIds" },
            new() { Title = "Roles-GetByTitle" },
            new() { Title = "Roles-Update" },
            new() { Title = "Startup-ApplyDatabaseChanges" },
            new() { Title = "Startup-DropAndCreateDatabase" },
            new() { Title = "Startup-CreateTables" },
            new() { Title = "Startup-DropTables" },
            new() { Title = "Startup-InsertCustomRecords" },
            new() { Title = "Startup-InsertDefaultRecords" },
        };

        var response = await _service.CreateCollectionAsync(entities);
        if (!response.Success)
            return response;

        return new SuccessResponse();
    }
}