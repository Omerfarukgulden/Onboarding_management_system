using FluentValidation;
using Onboard_management_system.OnboardingApplication.Dtos;


namespace Onboard_management_system.OnboardingApplication.Validators;

public class UpdateOnboardingTaskStatusDtoValidator : AbstractValidator<UpdateOnboardingTaskStatusDto>
{
    public UpdateOnboardingTaskStatusDtoValidator()
    {
        RuleFor(x=>x.NewStatus)
            .IsInEnum().WithMessage("Onboarding task status enumber is invalid");
    }
}