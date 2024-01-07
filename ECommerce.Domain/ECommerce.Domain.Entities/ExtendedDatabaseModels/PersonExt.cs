namespace ECommerce.Domain.Entities.ExtendedDatabaseModels;

public record PersonExt
{
    public required long PersonId { get; init; }
    public required short RoleId { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string CallingCode { get; init; }
    public required string Phone { get; init; }
    public string? RefreshToken { get; init; }
    public DateTimeOffset? RefreshTokenExpiryTime { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public required DateTimeOffset UpdatedAt { get; init; }

    // Extended With Role
    public required string RoleTitle { get; init; }
}
