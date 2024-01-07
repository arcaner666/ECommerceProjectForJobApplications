using ECommerce.Domain.Entities.Constants;
using ECommerce.Domain.Entities.DTOs.Person;
using FluentValidation;

namespace ECommerce.Presentation.Presentation.Validation.FluentValidation.Validators;

public class PersonTokenDtoValidator : AbstractValidator<PersonTokenDto>
{
    public PersonTokenDtoValidator()
    {
        RuleFor(p => p.RefreshToken).NotEmpty().WithMessage(Messages.Person_RefreshTokenIsNull);
        RuleFor(p => p.AccessToken).NotEmpty().WithMessage(Messages.Person_AccessTokenIsNull);
    }
}
