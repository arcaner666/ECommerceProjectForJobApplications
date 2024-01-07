using ECommerce.Domain.Entities.Constants;
using ECommerce.Domain.Entities.DTOs.Role;
using FluentValidation;

namespace ECommerce.Presentation.Presentation.Validation.FluentValidation.Validators;

public class RoleForCreateDtoValidator : AbstractValidator<RoleForCreateDto>
{
    public RoleForCreateDtoValidator()
    {
        RuleFor(r => r.Title)
            .NotEmpty().WithMessage(Messages.IsRequired(nameof(RoleForCreateDto.Title)));
        RuleFor(r => r.Detail)
            .NotEmpty().WithMessage(Messages.IsRequired(nameof(RoleForCreateDto.Detail)));
        RuleFor(r => r.ClaimForRoleDtos)
            .NotEmpty().WithMessage(Messages.IsRequired(nameof(RoleForCreateDto.ClaimForRoleDtos)));
    }
}
