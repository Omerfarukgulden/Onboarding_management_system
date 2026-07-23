using FluentValidation;
using Onboard_management_system.OnboardingApplication.Dtos;

namespace Onboard_management_system.OnboardingApplication.Validators;

public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
{
    public  UpdateUserDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("email boş olamaz")
            .EmailAddress().WithMessage("lüten geçerli bir email adresi girinz");

        RuleFor(x => x.Role)
            .IsInEnum().WithMessage("geçerli bir rol seçin ");

        RuleFor(x => x.DepartmentId)
            .GreaterThan(0)
            .When(x => x.DepartmentId.HasValue)
            .WithMessage("geçerli bir department girin");

    }
}