using Onboard_management_system.OnboardingDomain.Enums;

namespace Onboard_management_system.OnboardingApplication.Dtos;

public class OnboardingProcessDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public int OnboardingTemplateId { get; set; }
    public string? OnboardingTemplateName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public OnboardingStatus Status { get; set; }
}

public class StartOnboardingProcessDto
{
    public int EmployeeId { get; set; }
    public int OnboardingTemplateId { get; set; }
}