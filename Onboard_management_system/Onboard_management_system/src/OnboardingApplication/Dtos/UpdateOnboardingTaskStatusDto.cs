using TaskStatus = Onboard_management_system.OnboardingDomain.Enums.TaskStatus;

namespace Onboard_management_system.OnboardingApplication.Dtos;

// taskın durumunu günvellemek için gereken veriler
public class UpdateOnboardingTaskStatusDto
{
    public TaskStatus NewStatus { get; set; }
    
}