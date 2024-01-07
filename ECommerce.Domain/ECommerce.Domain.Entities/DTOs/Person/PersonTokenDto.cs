namespace ECommerce.Domain.Entities.DTOs.Person;

public record PersonTokenDto
{
    public required string RefreshToken { get; init; }
    public required string AccessToken { get; init; }
}
