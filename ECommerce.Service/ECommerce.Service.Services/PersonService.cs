using AutoMapper;
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.Constants;
using ECommerce.Domain.Entities.DatabaseModels;
using ECommerce.Domain.Entities.DTOs.Person;
using ECommerce.Domain.Entities.Responses;
using ECommerce.Domain.Entities.Responses.Extensions;
using ECommerce.Service.Contracts;
using ECommerce.Service.Services.Utilities.Security.Hashing;

namespace ECommerce.Service.Services;

public class PersonService : IPersonService
{
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

    public async Task<IResponse> CreateAsync(PersonForCreateDto personForCreateDto)
    {
        var searchedPerson =
            await _repository.Person.GetByEmailAsync(personForCreateDto.Email);
        if (searchedPerson is not null)
            return new AlreadyExistsResponse(Messages.Person_AlreadyExists);

        HashingHelper.CreatePasswordHash(personForCreateDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

        var personForCreate = _mapper.Map<Person>(personForCreateDto);
        var personForCreate2 = personForCreate with
        {
            Email = personForCreate.Email.ToLower(),
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow,
        };
        var person = await _repository.Person.CreateAsync(personForCreate2);

        var personDto = _mapper.Map<PersonDto>(person);

        return new SuccessDataResponse<PersonDto>(personDto);
    }

    public async Task<IResponse> CreateCollectionAsync(List<PersonForCreateDto> personForCreateDtos)
    {
        List<long> ids = new();
        List<PersonDto> personDtos = new();
        foreach (var personForCreateDto in personForCreateDtos)
        {
            var createPersonResponse = await CreateAsync(personForCreateDto);
            if (!createPersonResponse.Success)
                return createPersonResponse;

            var personDto = createPersonResponse.Convert<PersonDto>().Data;

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

    public async Task<IResponse> DeleteAsync(long id)
    {
        var getPersonResponse = await GetByIdAsync(id);
        if (!getPersonResponse.Success)
            return getPersonResponse;
        var personDto = getPersonResponse.Convert<PersonDto>().Data;

        var person = _mapper.Map<Person>(personDto);

        var deletePersonResult = await _repository.Person.DeleteAsync(person);
        if (!deletePersonResult)
            return new ErrorResponse(Messages.CanNotDeleted(nameof(Person)));

        return new SuccessResponse();
    }

    public async Task<IResponse> GetAllAsync()
    {
        var persons = await _repository.Person.GetAllAsync();
        if (!persons.Any())
            return new NotFoundResponse(Messages.Person_NotFound);

        var personDtos = _mapper.Map<List<PersonDto>>(persons);

        return new SuccessDataResponse<List<PersonDto>>(personDtos);
    }

    public async Task<IResponse> GetByIdAsync(long id)
    {
        if (id == 0)
            return new BadRequestResponse(Messages.Person_IdIsNull);

        var person = await _repository.Person.GetByIdAsync(id);
        if (person is null)
            return new NotFoundResponse(Messages.Person_NotFound);

        var personDto = _mapper.Map<PersonDto>(person);

        return new SuccessDataResponse<PersonDto>(personDto);
    }

    public async Task<IResponse> GetByIdsAsync(List<long> ids)
    {
        if (ids is null)
            return new BadRequestResponse(Messages.Person_IdsAreNull);

        var persons = await _repository.Person.GetByIdsAsync(ids);
        if (!persons.Any())
            return new NotFoundResponse(Messages.Person_NotFound);
        if (persons.Count() != ids.Count)
            return new BadRequestResponse(Messages.Person_CollectionCountMismatchComparingToIds);

        var personDtos = _mapper.Map<List<PersonDto>>(persons);

        return new SuccessDataResponse<List<PersonDto>>(personDtos);
    }

    public async Task<IResponse> GetByEmailAsync(string email)
    {
        var person = await _repository.Person.GetByEmailAsync(email);
        if (person is null)
            return new NotFoundResponse(Messages.Person_NotFound);

        var personDto = _mapper.Map<PersonDto>(person);

        return new SuccessDataResponse<PersonDto>(personDto);
    }

    public async Task<IResponse> GetExtByIdAsync(long id)
    {
        if (id == 0)
            return new BadRequestResponse(Messages.Person_IdIsNull);

        var personExt = await _repository.Person.GetExtByIdAsync(id);
        if (personExt is null)
            return new NotFoundResponse(Messages.Person_NotFound);

        var personExtDto = _mapper.Map<PersonExtDto>(personExt);

        return new SuccessDataResponse<PersonExtDto>(personExtDto);
    }

    public async Task<IResponse> GetExtsAsync()
    {
        var personExts = await _repository.Person.GetExtsAsync();
        if (!personExts.Any())
            return new NotFoundResponse(Messages.Person_NotFound);

        var personExtDtos = _mapper.Map<List<PersonExtDto>>(personExts);

        return new SuccessDataResponse<List<PersonExtDto>>(personExtDtos);
    }

    public async Task<IResponse> UpdateAfterLoginAsync(long id,
    PersonForUpdateAfterLoginDto personForUpdateAfterLoginDto)
    {
        if (id == 0)
            return new BadRequestResponse(Messages.Person_IdIsNull);

        var person = await _repository.Person.GetByIdAsync(id);
        if (person is null)
            return new NotFoundResponse(Messages.Person_NotFound);

        var person2 = person with
        {
            RefreshToken = personForUpdateAfterLoginDto.RefreshToken,
            RefreshTokenExpiryTime = personForUpdateAfterLoginDto.RefreshTokenExpiryTime,
            UpdatedAt = DateTimeOffset.UtcNow,
        };
        await _repository.Person.UpdateAsync(person2);

        return new SuccessResponse();
    }

    public async Task<IResponse> UpdateAsync(
        long id, PersonForUpdateDto personForUpdateDto)
    {
        if (id == 0)
            return new BadRequestResponse(Messages.Person_IdIsNull);

        var person = await _repository.Person.GetByIdAsync(id);
        if (person is null)
            return new NotFoundResponse(Messages.Person_NotFound);

        var person2 = person with
        {
            FirstName = personForUpdateDto.FirstName,
            LastName = personForUpdateDto.LastName,
            CallingCode = personForUpdateDto.CallingCode,
            Phone = personForUpdateDto.Phone,
            UpdatedAt = DateTimeOffset.UtcNow,
        };
        await _repository.Person.UpdateAsync(person2);

        return new SuccessResponse();
    }
}