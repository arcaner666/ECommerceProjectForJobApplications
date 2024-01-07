using ECommerce.Domain.Entities.Constants;
using ECommerce.Domain.Entities.DTOs.Role;
using FluentValidation;

namespace ECommerce.Presentation.Presentation.Validation.FluentValidation.Validators;

public class RoleForUpdateDtoValidator : AbstractValidator<RoleForUpdateDto>
{
    public RoleForUpdateDtoValidator()
    {
        RuleFor(x => x.RoleId)
            .NotEmpty().WithMessage(Messages.IsRequired(nameof(RoleForUpdateDto.RoleId)));
        RuleFor(r => r.Title)
            .NotEmpty().WithMessage(Messages.IsRequired(nameof(RoleForUpdateDto.Title)));
        RuleFor(r => r.Detail)
            .NotEmpty().WithMessage(Messages.IsRequired(nameof(RoleForUpdateDto.Detail)));
        RuleFor(x => x.ClaimDtos)
            .NotEmpty().WithMessage(Messages.IsRequired(nameof(RoleForUpdateDto.ClaimDtos)));
    }
}
