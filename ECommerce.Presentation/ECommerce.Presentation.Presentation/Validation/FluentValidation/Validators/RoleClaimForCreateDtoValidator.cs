using ECommerce.Domain.Entities.Constants;
using ECommerce.Domain.Entities.DTOs.RoleClaim;
using FluentValidation;

namespace ECommerce.Presentation.Presentation.Validation.FluentValidation.Validators;

public class RoleClaimForCreateDtoValidator : AbstractValidator<RoleClaimForCreateDto>
{
    public RoleClaimForCreateDtoValidator()
    {
        RuleFor(x => x.RoleId)
            .NotEmpty().WithMessage(Messages.IsRequired(nameof(RoleClaimForCreateDto.RoleId)));
        RuleFor(x => x.ClaimId)
            .NotEmpty().WithMessage(Messages.IsRequired(nameof(RoleClaimForCreateDto.ClaimId)));
    }
}
