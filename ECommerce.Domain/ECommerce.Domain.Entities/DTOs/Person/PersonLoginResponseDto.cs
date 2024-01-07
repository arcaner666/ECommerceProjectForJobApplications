using ECommerce.Domain.Entities.DTOs.RoleClaim;

namespace ECommerce.Domain.Entities.DTOs.Person;

public record PersonLoginResponseDto
{
    public required long PersonId { get; init; }
    public required long RoleId { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string CallingCode { get; init; }
    public required string Phone { get; init; }
    public required string RefreshToken { get; init; }
    public required DateTimeOffset RefreshTokenExpiryTime { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public required DateTimeOffset UpdatedAt { get; init; }

    // Extended With Role
    public required string RoleTitle { get; init; }

    // Extended
    public required string AccessToken { get; init; }
    public required List<RoleClaimExtDto> RoleClaimExtDtos { get; init; }
}
