using AutoMapper;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.Constants;
using ECommerce.Domain.Entities.DTOs.Person;
using ECommerce.Domain.Entities.DTOs.Role;
using ECommerce.Domain.Entities.DTOs.RoleClaim;
using ECommerce.Domain.Entities.Responses;
using ECommerce.Domain.Entities.Responses.Extensions;
using ECommerce.Service.Contracts;
using ECommerce.Service.Contracts.Extensions;
using ECommerce.Service.Services.Utilities.Security.Hashing;
using System.Security.Claims;

namespace ECommerce.Service.Services;

public class PersonAdvService : IPersonAdvService
{
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly IPersonService _personService;
    private readonly IRoleClaimService _roleClaimService;
    private readonly IRoleService _roleService;
    private readonly ITokenHelper _tokenHelper;

    public PersonAdvService(
        ILoggerManager logger,
        IMapper mapper,
        IPersonService personService,
        IRoleClaimService roleClaimService,
        IRoleService roleService,
        ITokenHelper tokenHelper
    )
    {
        _logger = logger;
        _mapper = mapper;
        _personService = personService;
        _roleClaimService = roleClaimService;
        _roleService = roleService;
        _tokenHelper = tokenHelper;
    }

    public async Task<IResponse> CreateAdminAsync(PersonExtForCreateDto personExtForCreateDto)
    {
        var getAdminRoleResponse = await _roleService.GetByTitleAsync("Admin");
        if (!getAdminRoleResponse.Success)
            return getAdminRoleResponse;
        var adminRoleDto = getAdminRoleResponse.Convert<RoleDto>().Data;

        var personForCreateDto = _mapper.Map<PersonForCreateDto>(personExtForCreateDto);
        var personForCreateDto2 = personForCreateDto with
        {
            RoleId = adminRoleDto.RoleId,
        };
        var createPersonResponse = await _personService.CreateAsync(personForCreateDto2);
        if (!createPersonResponse.Success)
            return createPersonResponse;
        var personDto = createPersonResponse.Convert<PersonDto>().Data;

        return new SuccessDataResponse<PersonDto>(personDto);
    }

    public async Task<IResponse> CreateUserAsync(PersonExtForCreateDto personExtForCreateDto)
    {
        var getRoleResponse = await _roleService.GetByTitleAsync("User");
        if (!getRoleResponse.Success)
            return getRoleResponse;
        var userRoleDto = getRoleResponse.Convert<RoleDto>().Data;

        var personForCreateDto = _mapper.Map<PersonForCreateDto>(personExtForCreateDto);
        var personForCreateDto2 = personForCreateDto with
        {
            RoleId = userRoleDto.RoleId
        };
        var createPersonResponse = await _personService.CreateAsync(personForCreateDto2);
        if (!createPersonResponse.Success)
            return createPersonResponse;
        var personDto = createPersonResponse.Convert<PersonDto>().Data;

        return new SuccessDataResponse<PersonDto>(personDto);
    }

    public async Task<IResponse> CreateUserCollectionAsync(List<PersonExtForCreateDto> personExtForCreateDtos)
    {
        List<long> ids = new();
        List<PersonDto> personDtos = new();
        foreach (var personExtForCreateDto in personExtForCreateDtos)
        {
            var createPersonUserResponse = await CreateUserAsync(personExtForCreateDto);
            if (!createPersonUserResponse.Success)
                return createPersonUserResponse;

            var personDto = createPersonUserResponse.Convert<PersonDto>().Data;

            personDtos.Add(personDto);
            ids.Add(personDto.PersonId);
        }

        var commaSeperatedStringIds = string.Join(",", ids);

        PersonCollectionDto personCollectionDto = new()
        {
            PersonDtos = personDtos,
            Ids = commaSeperatedStringIds,
        };

        return new SuccessDataResponse<PersonCollectionDto>(personCollectionDto);
    }

    public async Task<IResponse> LoginAsync(PersonLoginDto personLoginDto)
    {
        var getPersonResponse = await _personService.GetByEmailAsync(personLoginDto.Email.ToLower());
        if (!getPersonResponse.Success)
            return getPersonResponse;
        var personDto = getPersonResponse.Convert<PersonDto>().Data;

        if (!HashingHelper.VerifyPasswordHash(
                personLoginDto.Password,
                personDto.PasswordHash,
                personDto.PasswordSalt))
            return new ErrorResponse(Messages.Person_WrongPassword);

        var getRoleResponse = await _roleService.GetByIdAsync(personDto.RoleId);
        if (!getRoleResponse.Success)
            return getRoleResponse;
        var roleDto = getRoleResponse.Convert<RoleDto>().Data;

        var getRoleClaimResponse = await _roleClaimService.GetExtsByRoleIdAsync(roleDto.RoleId);
        if (!getRoleClaimResponse.Success)
            return getRoleClaimResponse;
        var roleClaimExtDtos = getRoleClaimResponse.Convert<List<RoleClaimExtDto>>().Data;

        var accessToken = _tokenHelper.GenerateAccessToken(
            personDto.PersonId,
            roleDto.Title,
            roleClaimExtDtos);
        var refreshToken = _tokenHelper.GenerateRefreshToken();

        var personForUpdateAfterLoginDto = new PersonForUpdateAfterLoginDto
        {
            RefreshToken = refreshToken,
            RefreshTokenExpiryTime = DateTimeOffset.UtcNow.AddSeconds(personLoginDto.RefreshTokenDuration)
        };
        var updatePersonResponse = await _personService.UpdateAfterLoginAsync(
            personDto.PersonId,
            personForUpdateAfterLoginDto);
        if (!updatePersonResponse.Success)
            return updatePersonResponse;

        var getUpdatedPersonResponse = await _personService.GetExtByIdAsync(personDto.PersonId);
        if (!getUpdatedPersonResponse.Success)
            return getUpdatedPersonResponse;
        var personExtDto = getUpdatedPersonResponse.Convert<PersonExtDto>().Data;

        var personLoginResponseDto = _mapper.Map<PersonLoginResponseDto>(personExtDto);
        var personLoginResponseDto2 = personLoginResponseDto with
        {
            AccessToken = accessToken,
            RoleTitle = roleDto.Title,
            RoleClaimExtDtos = roleClaimExtDtos,
        };

        return new SuccessDataResponse<PersonLoginResponseDto>(personLoginResponseDto2);
    }

    public async Task<IResponse> LogoutAsync(long id)
    {
        var getPersonResponse = await _personService.GetByIdAsync(id);
        if (!getPersonResponse.Success)
            return getPersonResponse;

        var personForUpdateAfterLoginDto = new PersonForUpdateAfterLoginDto
        {
            RefreshToken = null,
            RefreshTokenExpiryTime = null,
        };
        var updatePersonResponse = await _personService.UpdateAfterLoginAsync(id, personForUpdateAfterLoginDto);
        if (!updatePersonResponse.Success)
            return updatePersonResponse;

        return new SuccessResponse();
    }

    public async Task<IResponse> RefreshAccessTokenAsync(PersonTokenDto personTokenDto)
    {
        var getClaimsPrincipalResponse = _tokenHelper.GetPrincipalFromExpiredToken(personTokenDto.AccessToken);
        if (!getClaimsPrincipalResponse.Success)
            return getClaimsPrincipalResponse;
        var claimsPrincipal = getClaimsPrincipalResponse.Convert<ClaimsPrincipal>().Data;

        var role = claimsPrincipal.Role();

        var roleClaims = claimsPrincipal.RoleClaims();
        var roleClaimExtDtos = roleClaims.Select(roleClaim =>
            new RoleClaimExtDto
            {
                RoleClaimId = 0,
                RoleId = 0,
                ClaimId = 0,
                RoleTitle = role,
                ClaimTitle = roleClaim,
            }).ToList();

        var personId = claimsPrincipal.PersonId();
        var getPersonResponse = await _personService.GetByIdAsync(personId);
        if (!getPersonResponse.Success)
            return getPersonResponse;
        var personDto = getPersonResponse.Convert<PersonDto>().Data;

        if (personDto.RefreshToken != personTokenDto.RefreshToken)
            return new ErrorResponse(Messages.Person_TokenInvalid);
        if (personDto.RefreshTokenExpiryTime <= DateTimeOffset.UtcNow)
            return new ErrorResponse(Messages.Person_TokenExpired);

        var newAccessToken = _tokenHelper.GenerateAccessToken(
            personDto.PersonId,
            role,
            roleClaimExtDtos);

        var personTokenDtoResponse = personTokenDto with { AccessToken = newAccessToken };

        return new SuccessDataResponse<PersonTokenDto>(personTokenDtoResponse);
    }
}