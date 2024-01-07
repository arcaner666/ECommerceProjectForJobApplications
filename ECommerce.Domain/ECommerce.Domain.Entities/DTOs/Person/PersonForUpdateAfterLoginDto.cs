namespace ECommerce.Domain.Entities.DTOs.Person;

public record PersonForUpdateAfterLoginDto
{
    public string? RefreshToken { get; init; }
    public DateTimeOffset? RefreshTokenExpiryTime { get; init; }
}
