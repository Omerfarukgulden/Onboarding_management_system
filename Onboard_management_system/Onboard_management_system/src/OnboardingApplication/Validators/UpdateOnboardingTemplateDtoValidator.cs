using FluentValidation;
using Onboard_management_system.OnboardingApplication.Dtos;

namespace Onboard_management_system.OnboardingApplication.Validators;

public class UpdateOnboardingTemplateDtoValidator : AbstractValidator<UpdateOnboardingTemplateDto>
{
    public UpdateOnboardingTemplateDtoValidator()
    {
        RuleFor(y => y.IsActive)
            .IsInEnum().WithMessage("şablon durumunu giriniz");
    }
}