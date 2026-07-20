namespace Onboard_management_system.OnboardingApplication.Dtos;

public class OnboardingTemplateTaskDto
{
    public int Id { get; set; }
    public int OnboardingTemplateId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? ResponsibleDepartmentId { get; set; }
    public string? ResponsibleDepartmentName { get; set; }
    public bool IsMandatory { get; set; }
    public int DueInDays { get; set; }
}

public class CreateOnboardingTemplateTaskDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? ResponsibleDepartmentId { get; set; }
    public bool IsMandatory { get; set; }
    public int DueInDays { get; set; }
}