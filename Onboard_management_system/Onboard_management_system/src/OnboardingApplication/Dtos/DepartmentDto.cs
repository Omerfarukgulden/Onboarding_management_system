namespace Onboard_management_system.OnboardingApplication.Dtos;

public class DepartmentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}

public class CreateDepartmentDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class UpdateDepartmentDto : CreateDepartmentDto
{
    public bool IsActive { get; set; }
}