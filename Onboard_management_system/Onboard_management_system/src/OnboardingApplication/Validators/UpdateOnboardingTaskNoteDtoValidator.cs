using FluentValidation;
using Onboard_management_system.OnboardingApplication.Dtos;

namespace Onboard_management_system.OnboardingApplication.Validators;

public class UpdateOnboardingTaskNoteDtoValidator : AbstractValidator<UpdateOnboardingTaskNoteDto>
{
    public UpdateOnboardingTaskNoteDtoValidator()
    {
        RuleFor(x=>x.Note)
            .NotEmpty().WithMessage("Note is required");
    }
}