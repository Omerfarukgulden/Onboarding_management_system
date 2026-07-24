using FluentValidation;
using Onboard_management_system.OnboardingApplication.Dtos;

namespace Onboard_management_system.OnboardingApplication.Validators;

public class CreateOnboardingTemplateDtoValidator : AbstractValidator<CreateOnboardingTemplateDto>
{
    public CreateOnboardingTemplateDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("şablon ismi yazmanız bekleniyor");

        
    }
}