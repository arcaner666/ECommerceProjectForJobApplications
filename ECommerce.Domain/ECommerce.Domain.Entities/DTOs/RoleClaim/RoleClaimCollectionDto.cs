namespace ECommerce.Domain.Entities.DTOs.RoleClaim;

public record RoleClaimCollectionDto
{
    public List<RoleClaimDto> RoleClaimDtos { get; init; }
    public string Ids { get; init; }
}