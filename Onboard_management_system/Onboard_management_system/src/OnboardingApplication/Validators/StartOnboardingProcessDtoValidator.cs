using FluentValidation;
using Onboard_management_system.OnboardingApplication.Dtos;

namespace Onboard_management_system.OnboardingApplication.Validators;

public class StartOnboardingProcessDtoValidator : AbstractValidator<StartOnboardingProcessDto>
{
    public StartOnboardingProcessDtoValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("EmployeeId girmeniz bekleniyor");
        
        RuleFor(x=>x.OnboardingTemplateId)
            .NotEmpty().WithMessage("OnboardingTemplateId bekleniyor");
    }
}