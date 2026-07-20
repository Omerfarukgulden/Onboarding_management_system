namespace Onboard_management_system.OnboardingDomain.Entities;

public class OnboardingTemplateTask
{
    public int Id { get; set; }

    public int OnboardingTemplateId { get; set; }
    public OnboardingTemplate OnboardingTemplate { get; set; } = null!;

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    public int? ResponsibleDepartmentId { get; set; }
    public Department? ResponsibleDepartment { get; set; }

    public bool IsMandatory { get; set; }
    public int DueInDays { get; set; }
}