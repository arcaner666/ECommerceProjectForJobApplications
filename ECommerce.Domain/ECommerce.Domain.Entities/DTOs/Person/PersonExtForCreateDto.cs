namespace ECommerce.Domain.Entities.DTOs.Person;

public record PersonExtForCreateDto
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string CallingCode { get; init; }
    public required string Phone { get; init; }
    public required string Password { get; init; }
}
