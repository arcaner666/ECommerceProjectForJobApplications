namespace ECommerce.Domain.Entities.FilterModels;

public record RoleFilter
{
    public short? RoleId { get; init; }
    public string? Title { get; init; }
}
