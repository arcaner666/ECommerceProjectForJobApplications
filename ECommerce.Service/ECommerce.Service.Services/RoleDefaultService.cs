using AutoMapper;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.DTOs.Claim;
using ECommerce.Domain.Entities.DTOs.Role;
using ECommerce.Domain.Entities.FilterModels;
using ECommerce.Domain.Entities.Responses;
using ECommerce.Domain.Entities.Responses.Extensions;
using ECommerce.Service.Contracts;

namespace ECommerce.Service.Services;

public class RoleDefaultService : IRoleDefaultService
{
    private readonly IClaimService _claimService;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly IRoleService _roleService;

    public RoleDefaultService(
        IClaimService claimService,
        ILoggerManager logger,
        IMapper mapper,
        IRoleService roleService
    )
    {
        _claimService = claimService;
        _logger = logger;
        _mapper = mapper;
        _roleService = roleService;
    }

    public async Task<IResponse> InsertDefaultRoles()
    {
        var filter = new ClaimFilter();
        var claimResponse = await _claimService.GetListByFilterAsync(filter);
        if (!claimResponse.Success)
            return claimResponse;
        var claims = claimResponse.Convert<List<ClaimDto>>().Data;

        var claimsCreate = claims.Find(c => c.Title == "Claims-Create");
        var claimsCreateCollection = claims.Find(c => c.Title == "Claims-CreateCollection");
        var claimsDelete = claims.Find(c => c.Title == "Claims-Delete");
        var claimsGetListByFilter = claims.Find(c => c.Title == "Claims-GetListByFilter");
        var claimsGetById = claims.Find(c => c.Title == "Claims-GetById");
        var claimsGetByIds = claims.Find(c => c.Title == "Claims-GetByIds");
        var claimsGetSingleByFilter = claims.Find(c => c.Title == "Claims-GetSingleByFilter");
        var claimsUpdate = claims.Find(c => c.Title == "Claims-Update");
        var claimsUpdatePartially = claims.Find(c => c.Title == "Claims-UpdatePartially");
        var personsCreateAdmin = claims.Find(c => c.Title == "Persons-CreateAdmin");
        var personsCreateUser = claims.Find(c => c.Title == "Persons-CreateUser");
        var personsGetAll = claims.Find(c => c.Title == "Persons-GetAll");
        var personsGetById = claims.Find(c => c.Title == "Persons-GetById");
        var personsGetByIds = claims.Find(c => c.Title == "Persons-GetByIds");
        var personsGetExtById = claims.Find(c => c.Title == "Persons-GetExtById");
        var personsGetExts = claims.Find(c => c.Title == "Persons-GetExts");
        var personsLogin = claims.Find(c => c.Title == "Persons-Login");
        var personsLogout = claims.Find(c => c.Title == "Persons-Logout");
        var personsRefreshAccessToken = claims.Find(c => c.Title == "Persons-RefreshAccessToken");
        var personsUpdate = claims.Find(c => c.Title == "Persons-Update");
        var roleClaimsGetAll = claims.Find(c => c.Title == "RoleClaims-GetAll");
        var roleClaimsGetById = claims.Find(c => c.Title == "RoleClaims-GetById");
        var roleClaimsGetByIds = claims.Find(c => c.Title == "RoleClaims-GetByIds");
        var roleClaimsGetByRoleId = claims.Find(c => c.Title == "RoleClaims-GetByRoleId");
        var roleClaimsGetExtsByRoleId = claims.Find(c => c.Title == "RoleClaims-GetExtsByRoleId");
        var roleClaimsUpdate = claims.Find(c => c.Title == "RoleClaims-Update");
        var rolesCreate = claims.Find(c => c.Title == "Roles-Create");
        var rolesDelete = claims.Find(c => c.Title == "Roles-Delete");
        var rolesGetAll = claims.Find(c => c.Title == "Roles-GetAll");
        var rolesGetById = claims.Find(c => c.Title == "Roles-GetById");
        var rolesGetByIds = claims.Find(c => c.Title == "Roles-GetByIds");
        var rolesGetByTitle = claims.Find(c => c.Title == "Roles-GetByTitle");
        var rolesUpdate = claims.Find(c => c.Title == "Roles-Update");
        var startupApplyDatabaseChanges = claims.Find(c => c.Title == "Startup-ApplyDatabaseChanges");
        var startupDropAndCreateDatabase = claims.Find(c => c.Title == "Startup-DropAndCreateDatabase");
        var startupCreateTables = claims.Find(c => c.Title == "Startup-CreateTables");
        var startupDropTables = claims.Find(c => c.Title == "Startup-DropTables");
        var startupInsertCustomRecords = claims.Find(c => c.Title == "Startup-InsertCustomRecords");
        var startupInsertDefaultRecords = claims.Find(c => c.Title == "Startup-InsertDefaultRecords");

        var roleForManipulationDtos = new List<RoleForCreateDto>
        {
            new()
            {
                Title = "Admin",
                Detail = "It manages all system",
                ClaimForRoleDtos = new List<ClaimForRoleDto>
                {
                    new() { ClaimId = claimsCreate.ClaimId },
                    new() { ClaimId = claimsCreateCollection.ClaimId },
                    new() { ClaimId = claimsDelete.ClaimId },
                    new() { ClaimId = claimsGetListByFilter.ClaimId },
                    new() { ClaimId = claimsGetById.ClaimId },
                    new() { ClaimId = claimsGetByIds.ClaimId },
                    new() { ClaimId = claimsGetSingleByFilter.ClaimId },
                    new() { ClaimId = claimsUpdate.ClaimId },
                    new() { ClaimId = claimsUpdatePartially.ClaimId },
                    new() { ClaimId = personsCreateAdmin.ClaimId },
                    new() { ClaimId = personsCreateUser.ClaimId },
                    new() { ClaimId = personsGetAll.ClaimId },
                    new() { ClaimId = personsGetById.ClaimId },
                    new() { ClaimId = personsGetByIds.ClaimId },
                    new() { ClaimId = personsGetExtById.ClaimId },
                    new() { ClaimId = personsGetExts.ClaimId },
                    new() { ClaimId = personsLogin.ClaimId },
                    new() { ClaimId = personsLogout.ClaimId },
                    new() { ClaimId = personsRefreshAccessToken.ClaimId },
                    new() { ClaimId = personsUpdate.ClaimId },
                    new() { ClaimId = roleClaimsGetAll.ClaimId },
                    new() { ClaimId = roleClaimsGetById.ClaimId },
                    new() { ClaimId = roleClaimsGetByIds.ClaimId },
                    new() { ClaimId = roleClaimsGetByRoleId.ClaimId },
                    new() { ClaimId = roleClaimsGetExtsByRoleId.ClaimId },
                    new() { ClaimId = roleClaimsUpdate.ClaimId },
                    new() { ClaimId = rolesCreate.ClaimId },
                    new() { ClaimId = rolesDelete.ClaimId },
                    new() { ClaimId = rolesGetAll.ClaimId },
                    new() { ClaimId = rolesGetById.ClaimId },
                    new() { ClaimId = rolesGetByIds.ClaimId },
                    new() { ClaimId = rolesGetByTitle.ClaimId },
                    new() { ClaimId = rolesUpdate.ClaimId },
                    new() { ClaimId = startupApplyDatabaseChanges.ClaimId },
                    new() { ClaimId = startupDropAndCreateDatabase.ClaimId },
                    new() { ClaimId = startupCreateTables.ClaimId },
                    new() { ClaimId = startupDropTables.ClaimId },
                    new() { ClaimId = startupInsertCustomRecords.ClaimId },
                    new() { ClaimId = startupInsertDefaultRecords.ClaimId },
                }
            },
            new()
            {
                Title = "User",
                Detail = "It can only read data.",
                ClaimForRoleDtos = new List<ClaimForRoleDto>
                {
                    new() { ClaimId = personsGetAll.ClaimId },
                    new() { ClaimId = personsGetById.ClaimId },
                    new() { ClaimId = personsGetByIds.ClaimId },
                    new() { ClaimId = personsGetExtById.ClaimId },
                    new() { ClaimId = personsGetExts.ClaimId },
                    new() { ClaimId = personsLogin.ClaimId },
                    new() { ClaimId = personsLogout.ClaimId },
                    new() { ClaimId = personsRefreshAccessToken.ClaimId },
                }
            },
        };

        var roleResponse = await _roleService.CreateCollectionAsync(roleForManipulationDtos);
        if (!roleResponse.Success)
            return roleResponse;

        return new SuccessResponse();
    }
}