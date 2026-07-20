using Onboard_management_system.OnboardingDomain.Enums;

namespace Onboard_management_system.OnboardingApplication.Dtos;


//OnboardingProcess için tüm veriler 
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
//işe alışöa sürecinin gerektirdiği veriler 
public class StartOnboardingProcessDto
{
    public int EmployeeId { get; set; }
    public int OnboardingTemplateId { get; set; }
}