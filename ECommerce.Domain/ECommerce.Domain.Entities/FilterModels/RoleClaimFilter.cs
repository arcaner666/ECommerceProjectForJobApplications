namespace ECommerce.Domain.Entities.FilterModels;

public record RoleClaimFilter
{
    public long? RoleClaimId { get; init; }
    public short? RoleId { get; init; }
    public int? ClaimId { get; init; }
}
