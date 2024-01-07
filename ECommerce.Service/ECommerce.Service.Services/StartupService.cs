using AutoMapper;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.Responses;
using ECommerce.Service.Contracts;

namespace ECommerce.Service.Services;

public class StartupService : IStartupService
{
    private readonly IClaimDefaultService _claimDefaultService;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly IPersonCustomService _personCustomService;
    private readonly IRepositoryManager _repository;
    private readonly IRoleDefaultService _roleDefaultService;

    public StartupService(
        IClaimDefaultService claimDefaultService,
        ILoggerManager logger,
        IMapper mapper,
        IPersonCustomService personCustomService,
        IRepositoryManager repository,
        IRoleDefaultService roleDefaultService
    )
    {
        _claimDefaultService = claimDefaultService;
        _logger = logger;
        _mapper = mapper;
        _personCustomService = personCustomService;
        _repository = repository;
        _roleDefaultService = roleDefaultService;
    }

    public async Task<IResponse> ApplyDatabaseChanges()
    {
        await DropTables();
        await CreateTables();

        var insertDefaultRecordsResponse = await InsertDefaultRecords();
        if (!insertDefaultRecordsResponse.Success)
            return insertDefaultRecordsResponse;

        var insertCustomRecordsResponse = await InsertCustomRecords();
        if (!insertCustomRecordsResponse.Success)
            return insertCustomRecordsResponse;

        return new SuccessResponse();
    }

    public async Task<IResponse> DropAndCreateDatabase()
    {
        await _repository.Startup.DropAndCreateDatabase();
        return new SuccessResponse();
    }

    public async Task<IResponse> DropTables()
    {
        await _repository.Startup.DropTables();
        return new SuccessResponse();
    }

    public async Task<IResponse> CreateTables()
    {
        await _repository.Startup.CreateTableClaim();
        await _repository.Startup.CreateTableRole();
        await _repository.Startup.CreateTableRoleClaim();
        await _repository.Startup.CreateTablePerson();
        return new SuccessResponse();
    }

    public async Task<IResponse> InsertDefaultRecords()
    {
        var insertDefaultClaimsResponse = await _claimDefaultService.InsertDefaultClaims();
        if (!insertDefaultClaimsResponse.Success)
            return insertDefaultClaimsResponse;

        var insertRolesResponse = await _roleDefaultService.InsertDefaultRoles();
        if (!insertRolesResponse.Success)
            return insertRolesResponse;

        return new SuccessResponse();
    }

    public async Task<IResponse> InsertCustomRecords()
    {
        var insertCustomPersonsResponse = await _personCustomService.InsertCustomPersons();
        if (!insertCustomPersonsResponse.Success)
            return insertCustomPersonsResponse;

        return new SuccessResponse();
    }
}