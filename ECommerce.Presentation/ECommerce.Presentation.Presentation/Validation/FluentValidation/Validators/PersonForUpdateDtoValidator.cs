using ECommerce.Domain.Entities.Constants;
using ECommerce.Domain.Entities.DTOs.Person;
using ECommerce.Presentation.Presentation.Validation.FluentValidation.RuleBuilderExtentions;
using FluentValidation;

namespace ECommerce.Presentation.Presentation.Validation.FluentValidation.Validators;

public class PersonForUpdateDtoValidator : AbstractValidator<PersonForUpdateDto>
{
    public PersonForUpdateDtoValidator()
    {
        RuleFor(p => p.FirstName).NotEmpty().WithMessage(Messages.Person_FirstNameIsNull);
        RuleFor(p => p.LastName).NotEmpty().WithMessage(Messages.Person_LastNameIsNull);
        RuleFor(p => p.Phone)
            .NotEmpty().WithMessage(Messages.Person_PhoneIsNull)
            .NotValidPhone().WithMessage(Messages.Person_PhoneIsNotValid);
        RuleFor(p => p.CallingCode)
            .NotEmpty().WithMessage(Messages.Person_CallingCodeIsNull)
            .NotValidCallingCode().WithMessage(Messages.Person_CallingCodeIsNotValid);
    }
}
