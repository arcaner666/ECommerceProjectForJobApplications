using ECommerce.Domain.Entities.Constants;
using ECommerce.Domain.Entities.DTOs.Claim;
using FluentValidation;

namespace ECommerce.Presentation.Presentation.Validation.FluentValidation.Validators;

public class ClaimForManipulationDtoValidator : AbstractValidator<ClaimForManipulationDto>
{
    public ClaimForManipulationDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage(Messages.IsRequired(nameof(ClaimForManipulationDto.Title)));
    }
}
