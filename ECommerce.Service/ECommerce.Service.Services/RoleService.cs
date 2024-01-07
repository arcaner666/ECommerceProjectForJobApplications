using AutoMapper;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.Constants;
using ECommerce.Domain.Entities.DatabaseModels;
using ECommerce.Domain.Entities.DTOs.Role;
using ECommerce.Domain.Entities.DTOs.RoleClaim;
using ECommerce.Domain.Entities.FilterModels;
using ECommerce.Domain.Entities.Responses;
using ECommerce.Domain.Entities.Responses.Extensions;
using ECommerce.Service.Contracts;
using ECommerce.Service.Services.Utilities.Filtering;
using Microsoft.AspNetCore.JsonPatch;
using System.Linq.Expressions;

namespace ECommerce.Service.Services;

public class RoleService : IRoleService
{
    #region Constructor
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly IRepositoryManager _repository;
    private readonly IRoleClaimService _roleClaimService;

    public RoleService(
        ILoggerManager logger,
        IMapper mapper,
        IRepositoryManager repository,
        IRoleClaimService roleClaimService
    )
    {
        _logger = logger;
        _mapper = mapper;
        _repository = repository;
        _roleClaimService = roleClaimService;
    }
    #endregion

    #region Public Methods
    public async Task<IResponse> CreateAsync(RoleForCreateDto roleForManipulationDto)
    {
        var filter = new RoleFilter { Title = roleForManipulationDto.Title };
        var predicate = AndPredicate(filter);
        var searchedRole = await _repository.Role.GetByConditionAsync(predicate);
        if (searchedRole.Any())
            return new AlreadyExistsResponse(Messages.AlreadyExists(nameof(Role)));

        var role = _mapper.Map<Role>(roleForManipulationDto);

        var createdRole = await _repository.Role.CreateAsync(role);

        var roleClaimForCreateDtos =
            roleForManipulationDto.ClaimForRoleDtos.Select(claim =>
                new RoleClaimForCreateDto
                {
                    RoleId = createdRole.RoleId,
                    ClaimId = claim.ClaimId,
                }).ToList();
        var roleClaimResponse = await _roleClaimService.CreateCollectionAsync(roleClaimForCreateDtos);
        if (!roleClaimResponse.Success)
            return roleClaimResponse;

        var roleDto = _mapper.Map<RoleDto>(createdRole);

        return new SuccessDataResponse<RoleDto>(roleDto);
    }

    public async Task<IResponse> CreateCollectionAsync(List<RoleForCreateDto> entitiesForCreate)
    {
        List<short> ids = new();
        List<RoleDto> entityDtos = new();
        foreach (var entity in entitiesForCreate)
        {
            var response = await CreateAsync(entity);
            if (!response.Success)
                return response;
            var entityDto = response.Convert<RoleDto>().Data;

            entityDtos.Add(entityDto);
            ids.Add(entityDto.RoleId);
        }

        var commaSeperatedStringIds = string.Join(",", ids);

        RoleCollectionDto collectionDto = new()
        {
            RoleDtos = entityDtos,
            Ids = commaSeperatedStringIds,
        };

        return new SuccessDataResponse<RoleCollectionDto>(collectionDto);
    }

    public async Task<IResponse> DeleteAsync(short id)
    {
        var role = await _repository.Role.GetByIdAsync(
            new Role { RoleId = id });
        if (role is null)
            return new ErrorResponse(Messages.NotFound(nameof(Role)));

        var filter = new RoleClaimFilter { RoleId = id };
        var roleClaimResponse = await _roleClaimService.GetListByFilterAsync(filter);
        if (!roleClaimResponse.Success)
            return roleClaimResponse;
        var roleClaimDtos = roleClaimResponse.Convert<List<RoleClaimDto>>().Data;

        foreach (var roleClaim in roleClaimDtos)
        {
            var deleteRoleClaimResponse = await _roleClaimService.DeleteAsync(roleClaim.RoleClaimId);
            if (!deleteRoleClaimResponse.Success)
                return deleteRoleClaimResponse;
        }

        var result = await _repository.Role.DeleteAsync(role);
        if (!result)
            return new ErrorResponse(Messages.CanNotDeleted(nameof(Role)));

        return new SuccessResponse();
    }

    public async Task<IResponse> GetByIdAsync(short id)
    {
        if (id == 0)
            return new BadRequestResponse(Messages.IdIsNull(nameof(Role)));

        var entity = await _repository.Role.GetByIdAsync(
            new Role { RoleId = id, });
        if (entity is null)
            return new NotFoundResponse(Messages.NotFound(nameof(Role)));

        var entityDto = _mapper.Map<RoleDto>(entity);

        return new SuccessDataResponse<RoleDto>(entityDto);
    }

    public async Task<IResponse> GetByIdsAsync(List<short> ids)
    {
        if (!ids.Any())
            return new BadRequestResponse(Messages.IdsAreNull(nameof(Role)));

        var entities = await _repository.Role.GetByIdsAsync(ids);
        if (!entities.Any())
            return new NotFoundResponse(Messages.NotFound(nameof(Role)));
        if (entities.Count() != ids.Count)
            return new BadRequestResponse(Messages.CollectionCountMismatchComparingToIds(nameof(Role)));

        var entityDtos = _mapper.Map<List<RoleDto>>(entities);

        return new SuccessDataResponse<List<RoleDto>>(entityDtos);
    }

    public async Task<IResponse> GetListByFilterAsync(RoleFilter? filter)
    {
        if (filter is null)
        {
            var allEntities = await _repository.Role.GetAllAsync();
            if (!allEntities.Any())
                return new NotFoundResponse(Messages.NotFound(nameof(Role)));

            var allEntityDtos = _mapper.Map<List<RoleDto>>(allEntities);

            return new SuccessDataResponse<List<RoleDto>>(allEntityDtos);
        }

        var predicate = AndPredicate(filter);
        var entities = await _repository.Role.GetByConditionAsync(predicate);
        if (!entities.Any())
            return new NotFoundResponse(Messages.NotFound(nameof(Role)));

        var entityDtos = _mapper.Map<List<RoleDto>>(entities);

        return new SuccessDataResponse<List<RoleDto>>(entityDtos);
    }

    public async Task<IResponse> GetSingleByFilterAsync(RoleFilter filter)
    {
        var predicate = AndPredicate(filter);
        var entities = await _repository.Role.GetByConditionAsync(predicate);
        if (entities.Count() != 1)
            return new NotFoundResponse(Messages.NotFound(nameof(Role)));

        var entityDto = _mapper.Map<RoleDto>(entities.SingleOrDefault());

        return new SuccessDataResponse<RoleDto>(entityDto);
    }

    public async Task<IResponse> UpdateAsync(short id, RoleForUpdateDto entityForUpdate)
    {
        var entity = await _repository.Role.GetByIdAsync(
            new Role { RoleId = id });
        if (entity is null)
            return new ErrorResponse(Messages.NotFound(nameof(Role)));

        entity.Title = entityForUpdate.Title;
        entity.Detail = entityForUpdate.Detail;
        var updatedEntity = await _repository.Role.UpdateAsync(entity);
        var entityDto = _mapper.Map<RoleDto>(updatedEntity);

        var filter = new RoleClaimFilter { RoleId = entityForUpdate.RoleId };
        var roleClaimsResponse = await _roleClaimService.GetListByFilterAsync(filter);
        if (!roleClaimsResponse.Success)
            return roleClaimsResponse;
        var roleClaimDtos = roleClaimsResponse.Convert<List<RoleClaimDto>>().Data;

        foreach (var claimDto in entityForUpdate.ClaimDtos)
        {
            var roleClaim = roleClaimDtos.Find(roleClaim => roleClaim.ClaimId == claimDto.ClaimId);
            if (roleClaim is null)
            {
                RoleClaimForCreateDto roleClaimForCreateDto = new()
                {
                    RoleId = entityForUpdate.RoleId,
                    ClaimId = claimDto.ClaimId,
                };
                var createRoleClaimResponse = await _roleClaimService.CreateAsync(roleClaimForCreateDto);
                if (!createRoleClaimResponse.Success)
                    return createRoleClaimResponse;
            }
        }

        foreach (var roleClaimDto in roleClaimDtos)
        {
            var claimDto = entityForUpdate.ClaimDtos.Find(claim => claim.ClaimId == roleClaimDto.ClaimId);
            if (claimDto is null)
            {
                var deleteRoleClaimResponse = await _roleClaimService.DeleteAsync(roleClaimDto.RoleClaimId);
                if (!deleteRoleClaimResponse.Success)
                    return deleteRoleClaimResponse;
            }
        }

        return new SuccessDataResponse<RoleDto>(entityDto);
    }

    public async Task<IResponse> UpdatePartiallyAsync(
        short id, JsonPatchDocument<RoleForUpdateDto> entityForUpdateDtoPatchDoc)
    {
        var entity = await _repository.Role.GetByIdAsync(
            new Role { RoleId = id });
        if (entity is null)
            return new ErrorResponse(Messages.NotFound(nameof(Role)));

        var entityForUpdateDto = _mapper.Map<RoleForUpdateDto>(entity);
        entityForUpdateDtoPatchDoc.ApplyTo(entityForUpdateDto);

        entity.Title = entityForUpdateDto.Title;
        entity.Detail = entityForUpdateDto.Detail;
        var updatedEntity = await _repository.Role.UpdateAsync(entity);
        var entityDto = _mapper.Map<RoleDto>(updatedEntity);

        return new SuccessDataResponse<RoleDto>(entityDto);
    }
    #endregion

    #region Private Methods
    private Expression<Func<Role, bool>> AndPredicate(RoleFilter filter)
    {
        var predicate = PredicateBuilderHelper.True<Role>();
        if (filter.RoleId is not null)
            predicate = predicate.And(q => q.RoleId == filter.RoleId);
        if (filter.Title is not null)
            predicate = predicate.And(q => q.Title == filter.Title);
        return predicate;
    }
    #endregion
}
