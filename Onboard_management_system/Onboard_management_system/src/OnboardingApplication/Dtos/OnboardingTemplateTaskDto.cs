namespace Onboard_management_system.OnboardingApplication.Dtos;

//oluşturulmuş şablona oluşturulan görevi için veriler 
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

//Oşog oluşturmak için gerekli veriler 
public class CreateOnboardingTemplateTaskDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? ResponsibleDepartmentId { get; set; }
    public bool IsMandatory { get; set; }
    public int DueInDays { get; set; }
}