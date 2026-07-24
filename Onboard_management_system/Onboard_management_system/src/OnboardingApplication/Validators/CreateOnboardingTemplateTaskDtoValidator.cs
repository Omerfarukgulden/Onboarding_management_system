using FluentValidation;
using Onboard_management_system.OnboardingApplication.Dtos;

namespace Onboard_management_system.OnboardingApplication.Validators;

public class CreateOnboardingTemplateTaskDtoValidator : AbstractValidator<CreateOnboardingTemplateTaskDto>
{
    public CreateOnboardingTemplateTaskDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("title giriniz");

        RuleFor(x=>x.DueInDays)
            .GreaterThan(0).WithMessage("dueInDays giriniz");
        
    }
}