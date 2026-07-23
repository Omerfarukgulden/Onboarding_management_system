using FluentValidation;
using FluentValidation.Validators;
using Onboard_management_system.OnboardingApplication.Dtos;

namespace Onboard_management_system.OnboardingApplication.Validators;

public class CreateEmployeeDtoValidator : AbstractValidator<CreateEmployeeDto>
{
    public CreateEmployeeDtoValidator()
    {
        RuleFor(x => x.EmpName)
            .NotEmpty().WithMessage("isim boş olamaz")
            .MaximumLength(50);
        
        RuleFor(x => x.EmpSurname)
            .NotEmpty().WithMessage("soyisim boş olamaz")
            .MaximumLength(50);

        RuleFor(x => x.EmpEmail)
            .NotEmpty().WithMessage("email boş olamaz")
            .EmailAddress().WithMessage("geçerli email adresi girin ");

        RuleFor(x => x.EmpPhone)
            .NotEmpty().WithMessage("telefon numarası boş olamaz");

        RuleFor(x => x.HireDate)
            .NotEmpty().WithMessage("işe başlma tarihi boş olamaz")
            .LessThanOrEqualTo(DateTime.UtcNow.AddMonths(2))
            .WithMessage("işe başlama tarihi çok ileri bir tarih olamaz");

        RuleFor(x => x.DepartmentId)
            .GreaterThan(0).WithMessage("geçerli bir department id girin");
        
        RuleFor(x => x.PositionId)
            .GreaterThan(0).WithMessage("geçerli bir position id girin");


    }
}