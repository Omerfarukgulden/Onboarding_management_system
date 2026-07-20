using TaskStatus = Onboard_management_system.OnboardingDomain.Enums.TaskStatus;

namespace Onboard_management_system.OnboardingApplication.Dtos;

//Task için gereken veriler
public class OnboardingTaskDto
{
    public int Id { get; set; }
    public int OnboardingProcessId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? ResponsibleDepartmentId { get; set; }
    public string? ResponsibleDepartmentName { get; set; }
    public int? ResponsibleUserId { get; set; }
    public string? ResponsibleUserName { get; set; }
    public DateTime DueDate { get; set; }
    public TaskStatus Status { get; set; }
    public bool IsMandatory { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? Note { get; set; }
}