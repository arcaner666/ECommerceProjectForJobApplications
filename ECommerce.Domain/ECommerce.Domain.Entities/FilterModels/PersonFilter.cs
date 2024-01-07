namespace ECommerce.Domain.Entities.FilterModels;

public record PersonFilter
{
    public long? PersonId { get; init; }
    public short? RoleId { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Email { get; init; }
    public string? CallingCode { get; init; }
    public string? Phone { get; init; }
    public DateTimeOffset? CreatedAt { get; init; }
    public DateTimeOffset? UpdatedAt { get; init; }
}