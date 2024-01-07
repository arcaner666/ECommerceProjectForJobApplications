using ECommerce.Domain.Entities.Constants;
using ECommerce.Domain.Entities.DTOs.Person;
using ECommerce.Presentation.Presentation.Validation.FluentValidation.RuleBuilderExtentions;
using FluentValidation;

namespace ECommerce.Presentation.Presentation.Validation.FluentValidation.Validators;

public class PersonExtForCreateDtoValidator : AbstractValidator<PersonExtForCreateDto>
{
    public PersonExtForCreateDtoValidator()
    {
        RuleFor(p => p.FirstName)
            .NotEmpty().WithMessage(Messages.Person_FirstNameIsNull);
        RuleFor(p => p.LastName)
            .NotEmpty().WithMessage(Messages.Person_LastNameIsNull);
        RuleFor(p => p.Email)
            .NotEmpty().WithMessage(Messages.Person_EmailIsNull)
            .NotValidEmail().WithMessage(Messages.Person_EmailIsNotValid);
        RuleFor(p => p.CallingCode)
            .NotEmpty().WithMessage(Messages.Person_CallingCodeIsNull)
            .NotValidCallingCode().WithMessage(Messages.Person_CallingCodeIsNotValid);
        RuleFor(p => p.Phone)
            .NotEmpty().WithMessage(Messages.Person_PhoneIsNull)
            .NotValidPhone().WithMessage(Messages.Person_PhoneIsNotValid);
        RuleFor(p => p.Password)
            .NotEmpty().WithMessage(Messages.Person_PasswordIsNull)
            .NotValidPassword().WithMessage(Messages.Person_PasswordIsNotValid);
    }
}
