using AutoMapper;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.Constants;
using ECommerce.Domain.Entities.DatabaseModels;
using ECommerce.Domain.Entities.DTOs.Claim;
using ECommerce.Domain.Entities.FilterModels;
using ECommerce.Domain.Entities.Responses;
using ECommerce.Domain.Entities.Responses.Extensions;
using ECommerce.Service.Contracts;
using ECommerce.Service.Services.Utilities.Filtering;
using Microsoft.AspNetCore.JsonPatch;
using System.Linq.Expressions;

namespace ECommerce.Service.Services;

public class ClaimService : IClaimService
{
    #region Constructor
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly IRepositoryManager _repository;

    public ClaimService(
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
    public async Task<IResponse> CreateAsync(ClaimForManipulationDto entityForCreate)
    {
        var filter = new ClaimFilter { Title = entityForCreate.Title };
        var predicate = AndPredicate(filter);
        var searchedEntity = await _repository.Claim.GetByConditionAsync(predicate);
        if (searchedEntity.Any())
            return new AlreadyExistsResponse(Messages.AlreadyExists(nameof(Claim)));

        var entity = _mapper.Map<Claim>(entityForCreate);

        var createdEntity = await _repository.Claim.CreateAsync(entity);

        var entityDto = _mapper.Map<ClaimDto>(createdEntity);

        return new SuccessDataResponse<ClaimDto>(entityDto);
    }

    public async Task<IResponse> CreateCollectionAsync(List<ClaimForManipulationDto> entitiesForCreate)
    {
        List<int> ids = new();
        List<ClaimDto> entityDtos = new();
        foreach (var entity in entitiesForCreate)
        {
            var response = await CreateAsync(entity);
            if (!response.Success)
                return response;
            var entityDto = response.Convert<ClaimDto>().Data;

            entityDtos.Add(entityDto);
            ids.Add(entityDto.ClaimId);
        }

        var commaSeparatedStringIds = string.Join(",", ids);

        ClaimCollectionDto collectionDto = new()
        {
            ClaimDtos = entityDtos,
            Ids = commaSeparatedStringIds,
        };

        return new SuccessDataResponse<ClaimCollectionDto>(collectionDto);
    }

    public async Task<IResponse> DeleteAsync(int id)
    {
        var entity = await _repository.Claim.GetByIdAsync(
            new Claim { ClaimId = id });
        if (entity is null)
            return new ErrorResponse(Messages.NotFound(nameof(Claim)));

        var result = await _repository.Claim.DeleteAsync(entity);
        if (!result)
            return new ErrorResponse(Messages.CanNotDeleted(nameof(Claim)));

        return new SuccessResponse();
    }

    public async Task<IResponse> GetByIdAsync(int id)
    {
        if (id == 0)
            return new BadRequestResponse(Messages.IdIsNull(nameof(Claim)));

        var entity = await _repository.Claim.GetByIdAsync(
            new Claim { ClaimId = id, });
        if (entity is null)
            return new NotFoundResponse(Messages.NotFound(nameof(Claim)));

        var entityDto = _mapper.Map<ClaimDto>(entity);

        return new SuccessDataResponse<ClaimDto>(entityDto);
    }

    public async Task<IResponse> GetByIdsAsync(List<int> ids)
    {
        if (!ids.Any())
            return new BadRequestResponse(Messages.IdsAreNull(nameof(Claim)));

        var entities = await _repository.Claim.GetByIdsAsync(ids);
        if (!entities.Any())
            return new NotFoundResponse(Messages.NotFound(nameof(Claim)));
        if (entities.Count() != ids.Count)
            return new BadRequestResponse(Messages.CollectionCountMismatchComparingToIds(nameof(Claim)));

        var entityDtos = _mapper.Map<List<ClaimDto>>(entities);

        return new SuccessDataResponse<List<ClaimDto>>(entityDtos);
    }

    public async Task<IResponse> GetListByFilterAsync(ClaimFilter? filter)
    {
        if (filter is null)
        {
            var allEntities = await _repository.Claim.GetAllAsync();
            if (!allEntities.Any())
                return new NotFoundResponse(Messages.NotFound(nameof(Claim)));

            var allEntityDtos = _mapper.Map<List<ClaimDto>>(allEntities);

            return new SuccessDataResponse<List<ClaimDto>>(allEntityDtos);
        }

        var predicate = AndPredicate(filter);
        var entities = await _repository.Claim.GetByConditionAsync(predicate);
        if (!entities.Any())
            return new NotFoundResponse(Messages.NotFound(nameof(Claim)));

        var entityDtos = _mapper.Map<List<ClaimDto>>(entities);

        return new SuccessDataResponse<List<ClaimDto>>(entityDtos);
    }

    public async Task<IResponse> GetSingleByFilterAsync(ClaimFilter filter)
    {
        var predicate = AndPredicate(filter);
        var entities = await _repository.Claim.GetByConditionAsync(predicate);
        if (entities.Count() != 1)
            return new NotFoundResponse(Messages.NotFound(nameof(Claim)));

        var entityDto = _mapper.Map<ClaimDto>(entities.SingleOrDefault());

        return new SuccessDataResponse<ClaimDto>(entityDto);
    }

    public async Task<IResponse> UpdateAsync(int id, ClaimForManipulationDto entityForUpdate)
    {
        var entity = await _repository.Claim.GetByIdAsync(
            new Claim { ClaimId = id });
        if (entity is null)
            return new ErrorResponse(Messages.NotFound(nameof(Claim)));

        entity.Title = entityForUpdate.Title;
        var updatedEntity = await _repository.Claim.UpdateAsync(entity);
        var entityDto = _mapper.Map<ClaimDto>(updatedEntity);

        return new SuccessDataResponse<ClaimDto>(entityDto);
    }

    public async Task<IResponse> UpdatePartiallyAsync(
        int id, JsonPatchDocument<ClaimForManipulationDto> entityForUpdateDtoPatchDoc)
    {
        var entity = await _repository.Claim.GetByIdAsync(
            new Claim { ClaimId = id });
        if (entity is null)
            return new ErrorResponse(Messages.NotFound(nameof(Claim)));

        var entityForUpdateDto = _mapper.Map<ClaimForManipulationDto>(entity);
        entityForUpdateDtoPatchDoc.ApplyTo(entityForUpdateDto);

        entity.Title = entityForUpdateDto.Title;
        var updatedEntity = await _repository.Claim.UpdateAsync(entity);
        var entityDto = _mapper.Map<ClaimDto>(updatedEntity);

        return new SuccessDataResponse<ClaimDto>(entityDto);
    }
    #endregion

    #region Private Methods
    private Expression<Func<Claim, bool>> AndPredicate(ClaimFilter filter)
    {
        var predicate = PredicateBuilderHelper.True<Claim>();
        if (filter.ClaimId is not null)
            predicate = predicate.And(q => q.ClaimId == filter.ClaimId);
        if (filter.Title is not null)
            predicate = predicate.And(q => q.Title == filter.Title);
        return predicate;
    }

    private Expression<Func<Claim, bool>> OrPredicate(ClaimFilter filter)
    {
        var predicate = PredicateBuilderHelper.False<Claim>();
        if (filter.ClaimId is not null)
            predicate = predicate.Or(q => q.ClaimId == filter.ClaimId);
        if (filter.Title is not null)
            predicate = predicate.Or(q => q.Title == filter.Title);
        return predicate;
    }
    #endregion
}
