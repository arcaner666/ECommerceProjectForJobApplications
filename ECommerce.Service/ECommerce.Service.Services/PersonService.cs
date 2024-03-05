using AutoMapper;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.Constants;
using ECommerce.Domain.Entities.DatabaseModels;
using ECommerce.Domain.Entities.DTOs.Person;
using ECommerce.Domain.Entities.FilterModels;
using ECommerce.Domain.Entities.Responses;
using ECommerce.Domain.Entities.Responses.Extensions;
using ECommerce.Service.Contracts;
using ECommerce.Service.Services.Utilities.Filtering;
using ECommerce.Service.Services.Utilities.Security.Hashing;
using System.Linq.Expressions;

namespace ECommerce.Service.Services;

public class PersonService : IPersonService
{
    #region Constructor
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly IRepositoryManager _repository;

    public PersonService(
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
    public async Task<IResponse> CreateAsync(PersonForCreateDto entityForCreate)
    {
        var filter = new PersonFilter { Email = entityForCreate.Email };
        var predicate = AndPredicate(filter);
        var searchedEntity = await _repository.Person.GetByConditionAsync(predicate);
        if (searchedEntity.Any())
            return new AlreadyExistsResponse(Messages.AlreadyExists(nameof(Person)));

        HashingHelper.CreatePasswordHash(
            entityForCreate.Password, out var passwordHash, out var passwordSalt);

        var entity = _mapper.Map<Person>(entityForCreate);
        entity.Email = entityForCreate.Email.ToLower();
        entity.PasswordHash = passwordHash;
        entity.PasswordSalt = passwordSalt;
        entity.CreatedAt = DateTimeOffset.UtcNow;
        entity.UpdatedAt = DateTimeOffset.UtcNow;
        var createdEntity = await _repository.Person.CreateAsync(entity);

        var entityDto = _mapper.Map<PersonDto>(createdEntity);

        return new SuccessDataResponse<PersonDto>(entityDto);
    }

    public async Task<IResponse> CreateCollectionAsync(List<PersonForCreateDto> entitiesForCreate)
    {
        List<long> ids = new();
        List<PersonDto> entityDtos = new();
        foreach (var entity in entitiesForCreate)
        {
            var response = await CreateAsync(entity);
            if (!response.Success)
                return response;
            var entityDto = response.Convert<PersonDto>().Data;

            entityDtos.Add(entityDto);
            ids.Add(entityDto.PersonId);
        }

        var commaSeparatedStringIds = string.Join(",", ids);

        PersonCollectionDto collectionDto = new()
        {
            PersonDtos = entityDtos,
            Ids = commaSeparatedStringIds,
        };

        return new SuccessDataResponse<PersonCollectionDto>(collectionDto);
    }

    //public async Task<IResponse> CreateAdminAsync(PersonExtForCreateDto personExtForCreateDto)
    //{
    //    var getAdminRoleResponse = await _roleService.GetByTitleAsync("Admin");
    //    if (!getAdminRoleResponse.Success)
    //        return getAdminRoleResponse;
    //    var adminRoleDto = getAdminRoleResponse.Convert<RoleDto>().Data;

    //    var personForCreateDto = _mapper.Map<PersonForCreateDto>(personExtForCreateDto);
    //    var personForCreateDto2 = personForCreateDto with
    //    {
    //        RoleId = adminRoleDto.RoleId,
    //    };
    //    var createPersonResponse = await _personService.CreateAsync(personForCreateDto2);
    //    if (!createPersonResponse.Success)
    //        return createPersonResponse;
    //    var personDto = createPersonResponse.Convert<PersonDto>().Data;

    //    return new SuccessDataResponse<PersonDto>(personDto);
    //}

    //public async Task<IResponse> CreateUserAsync(PersonExtForCreateDto personExtForCreateDto)
    //{
    //    var getRoleResponse = await _roleService.GetByTitleAsync("User");
    //    if (!getRoleResponse.Success)
    //        return getRoleResponse;
    //    var userRoleDto = getRoleResponse.Convert<RoleDto>().Data;

    //    var personForCreateDto = _mapper.Map<PersonForCreateDto>(personExtForCreateDto);
    //    var personForCreateDto2 = personForCreateDto with
    //    {
    //        RoleId = userRoleDto.RoleId
    //    };
    //    var createPersonResponse = await _personService.CreateAsync(personForCreateDto2);
    //    if (!createPersonResponse.Success)
    //        return createPersonResponse;
    //    var personDto = createPersonResponse.Convert<PersonDto>().Data;

    //    return new SuccessDataResponse<PersonDto>(personDto);
    //}

    public async Task<IResponse> DeleteAsync(long id)
    {
        var entity = await _repository.Person.GetByIdAsync(
            new Person { PersonId = id });
        if (entity is null)
            return new ErrorResponse(Messages.NotFound(nameof(Person)));

        var result = await _repository.Person.DeleteAsync(entity);
        if (!result)
            return new ErrorResponse(Messages.CanNotDeleted(nameof(Person)));

        return new SuccessResponse();
    }

    public async Task<IResponse> GetByIdAsync(long id)
    {
        if (id == 0)
            return new BadRequestResponse(Messages.IdIsNull(nameof(Person)));

        var entity = await _repository.Person.GetByIdAsync(
            new Person { PersonId = id, });
        if (entity is null)
            return new NotFoundResponse(Messages.NotFound(nameof(Person)));

        var entityDto = _mapper.Map<PersonDto>(entity);

        return new SuccessDataResponse<PersonDto>(entityDto);
    }

    public async Task<IResponse> GetByIdsAsync(List<long> ids)
    {
        if (!ids.Any())
            return new BadRequestResponse(Messages.IdsAreNull(nameof(Person)));

        var entities = await _repository.Person.GetByIdsAsync(ids);
        if (!entities.Any())
            return new NotFoundResponse(Messages.NotFound(nameof(Person)));
        if (entities.Count() != ids.Count)
            return new BadRequestResponse(Messages.CollectionCountMismatchComparingToIds(nameof(Person)));

        var entityDtos = _mapper.Map<List<PersonDto>>(entities);

        return new SuccessDataResponse<List<PersonDto>>(entityDtos);
    }

    public async Task<IResponse> GetExtByIdAsync(long id)
    {
        if (id == 0)
            return new BadRequestResponse(Messages.IdIsNull(nameof(Person)));

        var entityExt = await _repository.Person.GetExtByIdAsync(id);
        if (entityExt is null)
            return new NotFoundResponse(Messages.NotFound(nameof(Person)));

        var entityExtDto = _mapper.Map<PersonExtDto>(entityExt);

        return new SuccessDataResponse<PersonExtDto>(entityExtDto);
    }

    public async Task<IResponse> GetExtsAsync()
    {
        var entityExts = await _repository.Person.GetExtsAsync();
        if (!entityExts.Any())
            return new NotFoundResponse(Messages.NotFound(nameof(Person)));

        var entityExtDtos = _mapper.Map<List<PersonExtDto>>(entityExts);

        return new SuccessDataResponse<List<PersonExtDto>>(entityExtDtos);
    }

    public async Task<IResponse> GetListByFilterAsync(PersonFilter? filter)
    {
        if (filter is null)
        {
            var allEntities = await _repository.Person.GetAllAsync();
            if (!allEntities.Any())
                return new NotFoundResponse(Messages.NotFound(nameof(Person)));

            var allEntityDtos = _mapper.Map<List<PersonDto>>(allEntities);

            return new SuccessDataResponse<List<PersonDto>>(allEntityDtos);
        }

        var predicate = AndPredicate(filter);
        var entities = await _repository.Person.GetByConditionAsync(predicate);
        if (!entities.Any())
            return new NotFoundResponse(Messages.NotFound(nameof(Person)));

        var entityDtos = _mapper.Map<List<PersonDto>>(entities);

        return new SuccessDataResponse<List<PersonDto>>(entityDtos);
    }

    public async Task<IResponse> GetSingleByFilterAsync(PersonFilter filter)
    {
        var predicate = AndPredicate(filter);
        var entities = await _repository.Person.GetByConditionAsync(predicate);
        if (entities.Count() != 1)
            return new NotFoundResponse(Messages.NotFound(nameof(Person)));

        var entityDto = _mapper.Map<PersonDto>(entities.SingleOrDefault());

        return new SuccessDataResponse<PersonDto>(entityDto);
    }

    //public async Task<IResponse> LoginAsync(PersonLoginDto personLoginDto)
    //{
    //    var filter = new PersonFilter { Email = personLoginDto.Email.ToLower() };
    //    var predicate = AndPredicate(filter);
    //    var person = await _repository.Person.GetByConditionAsync(predicate);
    //    if (!person.Any())
    //        return new NotFoundResponse(Messages.NotFound(nameof(Person)));

    //    if (!HashingHelper.VerifyPasswordHash(
    //            personLoginDto.Password,
    //            person.PasswordHash,
    //            person.PasswordSalt))
    //        return new ErrorResponse(Messages.Person_WrongPassword);

    //    var getRoleResponse = await _roleService.GetByIdAsync(personDto.RoleId);
    //    if (!getRoleResponse.Success)
    //        return getRoleResponse;
    //    var roleDto = getRoleResponse.Convert<RoleDto>().Data;

    //    var getRoleClaimResponse = await _roleClaimService.GetExtsByRoleIdAsync(roleDto.RoleId);
    //    if (!getRoleClaimResponse.Success)
    //        return getRoleClaimResponse;
    //    var roleClaimExtDtos = getRoleClaimResponse.Convert<List<RoleClaimExtDto>>().Data;

    //    var accessToken = _tokenHelper.GenerateAccessToken(
    //        personDto.PersonId,
    //        roleDto.Title,
    //        roleClaimExtDtos);
    //    var refreshToken = _tokenHelper.GenerateRefreshToken();

    //    var personForUpdateAfterLoginDto = new PersonForUpdateAfterLoginDto
    //    {
    //        RefreshToken = refreshToken,
    //        RefreshTokenExpiryTime = DateTimeOffset.UtcNow.AddSeconds(personLoginDto.RefreshTokenDuration)
    //    };
    //    var updatePersonResponse = await _personService.UpdateAfterLoginAsync(
    //        personDto.PersonId,
    //        personForUpdateAfterLoginDto);
    //    if (!updatePersonResponse.Success)
    //        return updatePersonResponse;

    //    var getUpdatedPersonResponse = await _personService.GetExtByIdAsync(personDto.PersonId);
    //    if (!getUpdatedPersonResponse.Success)
    //        return getUpdatedPersonResponse;
    //    var personExtDto = getUpdatedPersonResponse.Convert<PersonExtDto>().Data;

    //    var personLoginResponseDto = _mapper.Map<PersonLoginResponseDto>(personExtDto);
    //    var personLoginResponseDto2 = personLoginResponseDto with
    //    {
    //        AccessToken = accessToken,
    //        RoleTitle = roleDto.Title,
    //        RoleClaimExtDtos = roleClaimExtDtos,
    //    };

    //    return new SuccessDataResponse<PersonLoginResponseDto>(personLoginResponseDto2);
    //}

    //public async Task<IResponse> LogoutAsync(long id)
    //{
    //    var getPersonResponse = await _personService.GetByIdAsync(id);
    //    if (!getPersonResponse.Success)
    //        return getPersonResponse;

    //    var personForUpdateAfterLoginDto = new PersonForUpdateAfterLoginDto
    //    {
    //        RefreshToken = null,
    //        RefreshTokenExpiryTime = null,
    //    };
    //    var updatePersonResponse = await _personService.UpdateAfterLoginAsync(id, personForUpdateAfterLoginDto);
    //    if (!updatePersonResponse.Success)
    //        return updatePersonResponse;

    //    return new SuccessResponse();
    //}

    //public async Task<IResponse> RefreshAccessTokenAsync(PersonTokenDto personTokenDto)
    //{
    //    var getClaimsPrincipalResponse = _tokenHelper.GetPrincipalFromExpiredToken(personTokenDto.AccessToken);
    //    if (!getClaimsPrincipalResponse.Success)
    //        return getClaimsPrincipalResponse;
    //    var claimsPrincipal = getClaimsPrincipalResponse.Convert<ClaimsPrincipal>().Data;

    //    var role = claimsPrincipal.Role();

    //    var roleClaims = claimsPrincipal.RoleClaims();
    //    var roleClaimExtDtos = roleClaims.Select(roleClaim =>
    //        new RoleClaimExtDto
    //        {
    //            RoleClaimId = 0,
    //            RoleId = 0,
    //            ClaimId = 0,
    //            RoleTitle = role,
    //            ClaimTitle = roleClaim,
    //        }).ToList();

    //    var personId = claimsPrincipal.PersonId();
    //    var getPersonResponse = await _personService.GetByIdAsync(personId);
    //    if (!getPersonResponse.Success)
    //        return getPersonResponse;
    //    var personDto = getPersonResponse.Convert<PersonDto>().Data;

    //    if (personDto.RefreshToken != personTokenDto.RefreshToken)
    //        return new ErrorResponse(Messages.Person_TokenInvalid);
    //    if (personDto.RefreshTokenExpiryTime <= DateTimeOffset.UtcNow)
    //        return new ErrorResponse(Messages.Person_TokenExpired);

    //    var newAccessToken = _tokenHelper.GenerateAccessToken(
    //        personDto.PersonId,
    //        role,
    //        roleClaimExtDtos);

    //    var personTokenDtoResponse = personTokenDto with { AccessToken = newAccessToken };

    //    return new SuccessDataResponse<PersonTokenDto>(personTokenDtoResponse);
    //}

    public async Task<IResponse> UpdateAfterLoginAsync(long id,
    PersonForUpdateAfterLoginDto personForUpdateAfterLoginDto)
    {
        if (id == 0)
            return new BadRequestResponse(Messages.IdIsNull(nameof(Person)));

        var entity = await _repository.Person.GetByIdAsync(
            new Person { PersonId = id, });
        if (entity is null)
            return new NotFoundResponse(Messages.NotFound(nameof(Person)));

        entity.RefreshToken = personForUpdateAfterLoginDto.RefreshToken;
        entity.RefreshTokenExpiryTime = personForUpdateAfterLoginDto.RefreshTokenExpiryTime;
        entity.UpdatedAt = DateTimeOffset.UtcNow;
        await _repository.Person.UpdateAsync(entity);

        return new SuccessResponse();
    }

    public async Task<IResponse> UpdateAsync(long id, PersonForUpdateDto entityForUpdateDto)
    {
        if (id == 0)
            return new BadRequestResponse(Messages.IdIsNull(nameof(Person)));

        var entity = await _repository.Person.GetByIdAsync(
            new Person { PersonId = id, });
        if (entity is null)
            return new NotFoundResponse(Messages.NotFound(nameof(Person)));

        entity.FirstName = entityForUpdateDto.FirstName;
        entity.LastName = entityForUpdateDto.LastName;
        entity.CallingCode = entityForUpdateDto.CallingCode;
        entity.Phone = entityForUpdateDto.Phone;
        entity.UpdatedAt = DateTimeOffset.UtcNow;
        await _repository.Person.UpdateAsync(entity);

        return new SuccessResponse();
    }
    #endregion

    #region Private Methods
    private Expression<Func<Person, bool>> AndPredicate(PersonFilter filter)
    {
        var predicate = PredicateBuilderHelper.True<Person>();
        if (filter.PersonId is not null)
            predicate = predicate.And(q => q.PersonId == filter.PersonId);
        if (filter.RoleId is not null)
            predicate = predicate.And(q => q.RoleId == filter.RoleId);
        if (filter.FirstName is not null)
            predicate = predicate.And(q => q.FirstName == filter.FirstName);
        if (filter.LastName is not null)
            predicate = predicate.And(q => q.LastName == filter.LastName);
        if (filter.Email is not null)
            predicate = predicate.And(q => q.Email == filter.Email);
        if (filter.CallingCode is not null)
            predicate = predicate.And(q => q.CallingCode == filter.CallingCode);
        if (filter.Phone is not null)
            predicate = predicate.And(q => q.Phone == filter.Phone);
        if (filter.CreatedAt is not null)
            predicate = predicate.And(q => q.CreatedAt == filter.CreatedAt);
        if (filter.UpdatedAt is not null)
            predicate = predicate.And(q => q.UpdatedAt == filter.UpdatedAt);
        return predicate;
    }
    #endregion
}