namespace Onboard_management_system.OnboardingApplication.Dtos;

public class OnboardingTemplateDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public List<OnboardingTemplateTaskDto> TemplateTasks { get; set; } = new();
}

public class CreateOnboardingTemplateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class UpdateOnboardingTemplateDto : CreateOnboardingTemplateDto
{
    public bool IsActive { get; set; }
}
