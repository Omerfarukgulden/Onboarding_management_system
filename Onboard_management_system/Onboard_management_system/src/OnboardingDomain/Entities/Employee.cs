namespace Onboard_management_system.OnboardingDomain.Entities;

public class Employee
{
    public int EmpId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public DateTime HireDate { get; set; }

    public int DepartmentId { get; set; }
    public Department Department { get; set; } = null!;

    public int PositionId { get; set; }
    public Position Position { get; set; } = null!;

    public int? ManagerId { get; set; }
    public Employee? Manager { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<OnboardingProcess> OnboardingProcesses { get; set; } = new List<OnboardingProcess>();
}