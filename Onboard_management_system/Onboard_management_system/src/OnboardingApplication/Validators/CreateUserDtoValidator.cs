using FluentValidation;
using FluentValidation.Validators;
using Onboard_management_system.OnboardingApplication.Dtos;

namespace Onboard_management_system.OnboardingApplication.Validators;

public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
{
    public  CreateUserDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("email boş olamaz")
            .EmailAddress().WithMessage("uygun email adresi girin ");

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("kullanıcı adı boş olamaz")
            .MinimumLength(3).WithMessage("kullanıcı adı en az 3 karakter olmalı");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("şifre boş olamaz")
            .MinimumLength(6).WithMessage("şifre en az 6 karakter olmalı ");

    }
}