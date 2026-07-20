using TaskStatus = Onboard_management_system.OnboardingDomain.Enums.TaskStatus;

namespace Onboard_management_system.OnboardingApplication.Dtos;

public class UpdateOnboardingTaskStatusDto
{
    public TaskStatus NewStatus { get; set; }
    public int ChangedByUserId { get; set; }
}