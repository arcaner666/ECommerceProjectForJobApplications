using ECommerce.Domain.Entities.Constants;
using ECommerce.Domain.Entities.DTOs.Person;
using FluentValidation;

namespace ECommerce.Presentation.Presentation.Validation.FluentValidation.Validators;

public class PersonLoginDtoValidator : AbstractValidator<PersonLoginDto>
{
    public PersonLoginDtoValidator()
    {
        RuleFor(p => p.Email).NotEmpty().WithMessage(Messages.Person_EmailIsNull);
        RuleFor(p => p.Password).NotEmpty().WithMessage(Messages.Person_PasswordIsNull);
        RuleFor(p => p.RefreshTokenDuration).GreaterThanOrEqualTo(1).WithMessage(Messages.Person_RefreshTokenDurationIsLessThanOneSecond);
    }
}
