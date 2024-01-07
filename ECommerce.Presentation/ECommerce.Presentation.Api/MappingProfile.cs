using AutoMapper;
using ECommerce.Domain.Entities.DatabaseModels;
using ECommerce.Domain.Entities.DTOs.Claim;
using ECommerce.Domain.Entities.DTOs.Person;
using ECommerce.Domain.Entities.DTOs.Role;
using ECommerce.Domain.Entities.DTOs.RoleClaim;
using ECommerce.Domain.Entities.ExtendedDatabaseModels;

namespace ECommerce.Presentation.Api;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Claim, ClaimDto>().ReverseMap();
        CreateMap<Claim, ClaimForManipulationDto>().ReverseMap();
        CreateMap<ClaimDto, ClaimForManipulationDto>();

        CreateMap<Person, PersonDto>().ReverseMap();
        CreateMap<PersonDto, PersonForUpdateAfterLoginDto>();
        CreateMap<PersonExt, PersonExtDto>();
        CreateMap<PersonExtDto, PersonLoginResponseDto>();
        CreateMap<PersonExtForCreateDto, PersonForCreateDto>();
        CreateMap<PersonForCreateDto, Person>();

        CreateMap<Role, RoleDto>().ReverseMap();
        CreateMap<Role, RoleForCreateDto>().ReverseMap();

        CreateMap<RoleClaim, RoleClaimDto>().ReverseMap();
        CreateMap<RoleClaim, RoleClaimForCreateDto>().ReverseMap();
        CreateMap<RoleClaimExt, RoleClaimExtDto>().ReverseMap();
    }
}