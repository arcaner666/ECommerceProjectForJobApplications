using ECommerce.Domain.Entities.DTOs.Claim;

namespace ECommerce.Domain.Entities.DTOs.Role;

public record RoleForUpdateDto
{
    public short RoleId { get; init; }
    public string Title { get; init; }
    public string Detail { get; init; }
    public List<ClaimDto> ClaimDtos { get; init; }
}