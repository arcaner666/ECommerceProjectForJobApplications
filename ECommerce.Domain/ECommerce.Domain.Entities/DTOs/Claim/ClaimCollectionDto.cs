namespace ECommerce.Domain.Entities.DTOs.Claim;

public record ClaimCollectionDto
{
    public required List<ClaimDto> ClaimDtos { get; init; }
    public required string Ids { get; init; }
}
