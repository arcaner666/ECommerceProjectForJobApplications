using AutoMapper;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.Constants;
using ECommerce.Domain.Entities.DatabaseModels;
using ECommerce.Domain.Entities.DTOs.RoleClaim;
using ECommerce.Domain.Entities.FilterModels;
using ECommerce.Domain.Entities.Responses;
using ECommerce.Domain.Entities.Responses.Extensions;
using ECommerce.Service.Contracts;
using ECommerce.Service.Services.Utilities.Filtering;
using System.Linq.Expressions;

namespace ECommerce.Service.Services;

public class RoleClaimService : IRoleClaimService
{
    #region Constructor
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly IRepositoryManager _repository;

    public RoleClaimService(
        ILoggerManager logger,
        IMapper mapper,
        IRepositoryManager repository
    )
    {
        _logger = logger;
        _mapper = mapper;
        _repository = repository;
    }
    #endregion

    #region Public Methods
    public async Task<IResponse> CreateAsync(RoleClaimForCreateDto roleClaimForCreateDto)
    {
        var filter = new RoleClaimFilter
        {
            RoleId = roleClaimForCreateDto.RoleId,
            ClaimId = roleClaimForCreateDto.ClaimId,
        };
        var predicate = AndPredicate(filter);
        var searchedClaim = await _repository.RoleClaim.GetByConditionAsync(predicate);
        if (searchedClaim.Any())
            return new AlreadyExistsResponse(Messages.AlreadyExists(nameof(RoleClaim)));

        var roleClaim = _mapper.Map<RoleClaim>(roleClaimForCreateDto);

        var createdRoleClaim = await _repository.RoleClaim.CreateAsync(roleClaim);

        var roleClaimDto = _mapper.Map<RoleClaimDto>(createdRoleClaim);

        return new SuccessDataResponse<RoleClaimDto>(roleClaimDto);
    }

    public async Task<IResponse> CreateCollectionAsync(List<RoleClaimForCreateDto> roleClaimForCreateDtos)
    {
        List<long> ids = new();
        List<RoleClaimDto> roleClaimDtos = new();
        foreach (var roleClaimForCreateDto in roleClaimForCreateDtos)
        {
            var roleClaimResponse = await CreateAsync(roleClaimForCreateDto);
            if (!roleClaimResponse.Success)
                return roleClaimResponse;

            var roleClaimDto = roleClaimResponse.Convert<RoleClaimDto>().Data;

            roleClaimDtos.Add(roleClaimDto);
            ids.Add(roleClaimDto.RoleClaimId);
        }

        var commaSeperatedStringIds = string.Join(",", ids);

        RoleClaimCollectionDto roleClaimCollectionDto = new()
        {
            RoleClaimDtos = roleClaimDtos,
            Ids = commaSeperatedStringIds,
        };

        return new SuccessDataResponse<RoleClaimCollectionDto>(roleClaimCollectionDto);
    }

    public async Task<IResponse> DeleteAsync(long id)
    {
        var searchedRoleClaim = await _repository.RoleClaim.GetByIdAsync(
            new RoleClaim { RoleClaimId = id });
        if (searchedRoleClaim is null)
            return new ErrorResponse(Messages.NotFound(nameof(RoleClaim)));

        var result = await _repository.RoleClaim.DeleteAsync(searchedRoleClaim);
        if (!result)
            return new ErrorResponse(Messages.CanNotDeleted(nameof(RoleClaim)));

        return new SuccessResponse();
    }

    public async Task<IResponse> GetByIdAsync(long id)
    {
        if (id == 0)
            return new BadRequestResponse(Messages.IdIsNull(nameof(RoleClaim)));

        var roleClaim = await _repository.RoleClaim.GetByIdAsync(
            new RoleClaim { RoleClaimId = id, });
        if (roleClaim is null)
            return new NotFoundResponse(Messages.NotFound(nameof(RoleClaim)));

        var roleClaimDto = _mapper.Map<RoleClaimDto>(roleClaim);

        return new SuccessDataResponse<RoleClaimDto>(roleClaimDto);
    }

    public async Task<IResponse> GetByIdsAsync(List<long> ids)
    {
        if (!ids.Any())
            return new BadRequestResponse(Messages.IdsAreNull(nameof(RoleClaim)));

        var roleClaims = await _repository.RoleClaim.GetByIdsAsync(ids);
        if (!roleClaims.Any())
            return new NotFoundResponse(Messages.NotFound(nameof(RoleClaim)));
        if (roleClaims.Count() != ids.Count)
            return new BadRequestResponse(Messages.CollectionCountMismatchComparingToIds(nameof(RoleClaim)));

        var roleClaimDtos = _mapper.Map<List<RoleClaimDto>>(roleClaims);

        return new SuccessDataResponse<List<RoleClaimDto>>(roleClaimDtos);
    }

    public async Task<IResponse> GetExtsByRoleIdAsync(short roleId)
    {
        if (roleId == 0)
            return new BadRequestResponse(Messages.RoleClaim_RoleIdIsNull);

        var roleClaimExts = await _repository.RoleClaim.GetExtsByRoleIdAsync(roleId);
        if (!roleClaimExts.Any())
            return new NotFoundResponse(Messages.RoleClaim_NotFound);

        var roleClaimExtDtos = _mapper.Map<List<RoleClaimExtDto>>(roleClaimExts);

        return new SuccessDataResponse<List<RoleClaimExtDto>>(roleClaimExtDtos);
    }

    public async Task<IResponse> GetListByFilterAsync(RoleClaimFilter? filter)
    {
        if (filter is null)
        {
            var allRoleClaims = await _repository.RoleClaim.GetAllAsync();
            if (!allRoleClaims.Any())
                return new NotFoundResponse(Messages.NotFound(nameof(RoleClaim)));

            var allRoleClaimDtos = _mapper.Map<List<RoleClaimDto>>(allRoleClaims);

            return new SuccessDataResponse<List<RoleClaimDto>>(allRoleClaimDtos);
        }

        var predicate = AndPredicate(filter);
        var roleClaims = await _repository.RoleClaim.GetByConditionAsync(predicate);
        if (!roleClaims.Any())
            return new NotFoundResponse(Messages.NotFound(nameof(RoleClaim)));

        var roleClaimDtos = _mapper.Map<List<RoleClaimDto>>(roleClaims);

        return new SuccessDataResponse<List<RoleClaimDto>>(roleClaimDtos);
    }

    public async Task<IResponse> GetSingleByFilterAsync(RoleClaimFilter filter)
    {
        var predicate = AndPredicate(filter);
        var roleClaims = await _repository.RoleClaim.GetByConditionAsync(predicate);
        if (roleClaims.Count() != 1)
            return new NotFoundResponse(Messages.NotFound(nameof(RoleClaim)));

        var roleClaimDto = _mapper.Map<RoleClaimDto>(roleClaims.SingleOrDefault());

        return new SuccessDataResponse<RoleClaimDto>(roleClaimDto);
    }
    #endregion

    #region Private Methods
    private Expression<Func<RoleClaim, bool>> AndPredicate(RoleClaimFilter filter)
    {
        var predicate = PredicateBuilderHelper.True<RoleClaim>();
        if (filter.RoleClaimId is not null)
            predicate = predicate.And(q => q.ClaimId == filter.RoleClaimId);
        if (filter.RoleId is not null)
            predicate = predicate.And(q => q.ClaimId == filter.RoleId);
        if (filter.ClaimId is not null)
            predicate = predicate.And(q => q.ClaimId == filter.ClaimId);
        return predicate;
    }
    #endregion
}
