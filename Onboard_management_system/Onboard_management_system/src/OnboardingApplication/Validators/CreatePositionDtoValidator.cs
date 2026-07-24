using FluentValidation;
using Onboard_management_system.OnboardingApplication.Dtos;

namespace Onboard_management_system.OnboardingApplication.Validators;

public class CreatePositionDtoValidator : AbstractValidator<CreatePositionDto>
{
    public CreatePositionDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("pozisyon adı giriniz");

       
        
    }
}