using TaskStatus = Onboard_management_system.OnboardingDomain.Enums.TaskStatus;

namespace Onboard_management_system.OnboardingApplication.Dtos;


//task durumuu tarihi için gerkeen veriler
public class TaskStatusHistoryDto
{
    public int Id { get; set; }
    public int OnboardingTaskId { get; set; }
    public TaskStatus OldStatus { get; set; }
    public TaskStatus NewStatus { get; set; }
    public int ChangedByUserId { get; set; }
    public string? ChangedByUsername { get; set; }
    public DateTime ChangedAt { get; set; }
}