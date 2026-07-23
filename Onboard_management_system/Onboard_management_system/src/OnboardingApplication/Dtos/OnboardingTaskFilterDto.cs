using Onboard_management_system.OnboardingApplication.Common;
using TaskStatus = Onboard_management_system.OnboardingDomain.Enums.TaskStatus;

namespace Onboard_management_system.OnboardingApplication.Dtos;

public class OnboardingTaskFilterDto : PaginationParams
{
    public TaskStatus? Status { get; set; }
    public int? ResponsibleDepartmentId { get; set; }
    public int? ResponsibleUserId { get; set; }
    public bool? IsMandatory { get; set; }
}