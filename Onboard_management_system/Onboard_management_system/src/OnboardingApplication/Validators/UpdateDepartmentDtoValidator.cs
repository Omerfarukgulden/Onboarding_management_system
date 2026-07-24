using FluentValidation;
using Onboard_management_system.OnboardingApplication.Dtos;

namespace Onboard_management_system.OnboardingApplication.Validators;

public class UpdateDepartmentDtoValidator : AbstractValidator<UpdateDepartmentDto>
{
    public UpdateDepartmentDtoValidator()
    {
       Include(new CreateDepartmentDtoValidator());
    }
}