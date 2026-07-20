using OnboardingStatus = Onboard_management_system.OnboardingDomain.Enums.OnboardingStatus;

namespace Onboard_management_system.OnboardingDomain.Entities;

public class OnboardingProcess
{
    //onboarding proccess bilgileri 
    public int Id { get; set; }

    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    public int OnboardingTemplateId { get; set; }
    public OnboardingTemplate OnboardingTemplate { get; set; } = null!;

    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public OnboardingStatus Status { get; set; }
    
    // inheritince kısmı 

    public ICollection<OnboardingTask> Tasks { get; set; } = new List<OnboardingTask>();
}