using FluentValidation;
using Onboard_management_system.OnboardingApplication.Dtos;

namespace Onboard_management_system.OnboardingApplication.Validators;

public class CreateOnboardingTemplateTaskDtoValidator : AbstractValidator<CreateOnboardingTemplateTaskDto>
{
    public CreateOnboardingTemplateTaskDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("title giriniz");
        
        RuleFor(y => y.Description)
            .NotEmpty().WithMessage("description giriniz");
        
        RuleFor(x => x.ResponsibleDepartmentId)
            .NotEmpty().WithMessage("responsibleDepartmentId giriniz");
        
        RuleFor(x=>x.DueInDays)
            .GreaterThan(0).WithMessage("dueInDays giriniz");
        
    }
}