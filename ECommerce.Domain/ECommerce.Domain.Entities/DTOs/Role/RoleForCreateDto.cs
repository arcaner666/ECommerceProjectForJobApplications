using ECommerce.Domain.Entities.DTOs.Claim;

namespace ECommerce.Domain.Entities.DTOs.Role;

public record RoleForCreateDto
{
    public string Title { get; init; }
    public string Detail { get; init; }

    // Extended With Claim
    public List<ClaimForRoleDto> ClaimForRoleDtos { get; init; }
}