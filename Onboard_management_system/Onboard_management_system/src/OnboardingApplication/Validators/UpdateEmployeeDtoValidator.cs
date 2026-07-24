using FluentValidation;
using Onboard_management_system.OnboardingApplication.Dtos;

namespace Onboard_management_system.OnboardingApplication.Validators;

public class UpdateEmployeeDtoValidator : AbstractValidator<UpdateEmployeeDto>
{
    public  UpdateEmployeeDtoValidator()
    {
        Include(new CreateEmployeeDtoValidator());

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.HireDate)
            .When(x => x.EndDate.HasValue)
            .WithMessage("işten çıkış tarihi giriş tarihinden sonra olmalı");
    }
}