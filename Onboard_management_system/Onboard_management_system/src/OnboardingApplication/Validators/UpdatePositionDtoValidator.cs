using FluentValidation;
using Onboard_management_system.OnboardingApplication.Dtos;

namespace Onboard_management_system.OnboardingApplication.Validators;

public class UpdatePositionDtoValidator : AbstractValidator<UpdatePositionDto>
{
    public  UpdatePositionDtoValidator()
    {
        Include(new CreatePositionDtoValidator());
    }
}