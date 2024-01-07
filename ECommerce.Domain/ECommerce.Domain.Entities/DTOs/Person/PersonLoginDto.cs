namespace ECommerce.Domain.Entities.DTOs.Person;

public record PersonLoginDto
{
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required int RefreshTokenDuration { get; init; }
}
