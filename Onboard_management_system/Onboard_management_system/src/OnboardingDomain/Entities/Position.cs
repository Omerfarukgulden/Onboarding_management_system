namespace Onboard_management_system.OnboardingDomain.Entities;

public class Position
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}