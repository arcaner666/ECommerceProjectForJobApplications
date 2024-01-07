namespace ECommerce.Domain.Entities.DTOs.RoleClaim;

public record RoleClaimForCreateDto
{
    public short RoleId { get; init; }
    public int ClaimId { get; init; }
}