namespace Onboard_management_system.OnboardingDomain.Entities;

public class OnboardingTemplate
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<OnboardingTemplateTask> TemplateTasks { get; set; } = new List<OnboardingTemplateTask>();
}