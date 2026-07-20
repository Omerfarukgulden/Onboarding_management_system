namespace Onboard_management_system.OnboardingDomain.Entities;

public class Department
{
    // department bilgileri 
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    
    //inheritince kısmı

    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    public ICollection<User> Users { get; set; } = new List<User>();
}