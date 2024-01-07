namespace ECommerce.Domain.Entities.DTOs.Role;

public record RoleCollectionDto
{
    public List<RoleDto> RoleDtos { get; init; }
    public string Ids { get; init; }
}