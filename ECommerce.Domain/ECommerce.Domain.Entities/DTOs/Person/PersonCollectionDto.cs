namespace ECommerce.Domain.Entities.DTOs.Person;

public record PersonCollectionDto
{
    public required List<PersonDto> PersonDtos { get; init; }
    public required string Ids { get; init; }
}
