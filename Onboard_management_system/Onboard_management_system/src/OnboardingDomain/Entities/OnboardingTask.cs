using TaskStatus = Onboard_management_system.OnboardingDomain.Enums.TaskStatus;

namespace Onboard_management_system.OnboardingDomain.Entities;

public class OnboardingTask
{
    public int Id { get; set; }

    public int OnboardingProcessId { get; set; }
    public OnboardingProcess OnboardingProcess { get; set; } = null!;

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    public int? ResponsibleDepartmentId { get; set; }
    public Department? ResponsibleDepartment { get; set; }

    public int? ResponsibleUserId { get; set; }
    public User? ResponsibleUser { get; set; }

    public DateTime DueDate { get; set; }
    public TaskStatus Status { get; set; }
    public bool IsMandatory { get; set; }

    public DateTime? CompletedAt { get; set; }
    public int? CompletedByUserId { get; set; }
    public User? CompletedByUser { get; set; }

    public string? Note { get; set; }

    public ICollection<TaskStatusHistory> StatusHistory { get; set; } = new List<TaskStatusHistory>();
}