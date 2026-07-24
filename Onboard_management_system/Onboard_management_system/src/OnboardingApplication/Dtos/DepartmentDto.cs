namespace Onboard_management_system.OnboardingApplication.Dtos;

//department entitysi için tüm veriler 
public class DepartmentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}
//department oluşturmak için gereken veriler 
public class CreateDepartmentDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
// departent güncellemek için gereken veriler 
public class UpdateDepartmentDto : CreateDepartmentDto
{
    public UpdateDepartmentDto(bool ısActive)
    {
        IsActive = ısActive;
    }

    public bool IsActive { get; set; }
}