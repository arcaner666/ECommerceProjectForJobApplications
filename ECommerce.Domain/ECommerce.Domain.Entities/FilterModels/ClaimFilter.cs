namespace ECommerce.Domain.Entities.FilterModels;

public record ClaimFilter
{
    public int? ClaimId { get; init; }
    public string? Title { get; init; }
}
