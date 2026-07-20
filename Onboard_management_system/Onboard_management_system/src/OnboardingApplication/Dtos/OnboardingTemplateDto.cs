namespace Onboard_management_system.OnboardingApplication.Dtos;


//şablon için gereken veriler
public class OnboardingTemplateDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public List<OnboardingTemplateTaskDto> TemplateTasks { get; set; } = new();
}


//şablon oluşturmak içib girilen veriler 
public class CreateOnboardingTemplateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

//şablon güncellemek için girilen veriler 
public class UpdateOnboardingTemplateDto : CreateOnboardingTemplateDto
{
    public bool IsActive { get; set; }
}
