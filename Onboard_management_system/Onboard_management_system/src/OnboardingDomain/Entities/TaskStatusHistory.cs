using TaskStatus = Onboard_management_system.OnboardingDomain.Enums.TaskStatus;

namespace Onboard_management_system.OnboardingDomain.Entities;

public class TaskStatusHistory
{
    public int Id { get; set; }

    public int OnboardingTaskId { get; set; }
    public OnboardingTask OnboardingTask { get; set; } = null!;

    public TaskStatus OldStatus { get; set; }
    public TaskStatus NewStatus { get; set; }

    public int ChangedByUserId { get; set; }
    public User ChangedByUser { get; set; } = null!;

    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
}