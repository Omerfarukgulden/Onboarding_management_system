namespace Onboard_management_system.OnboardingDomain.Entities;

public class Employee
{
    //employee bilgileri 
    public int EmpId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string EmpAddress { get; set; } = string.Empty;
    public string EmpBlood { get; set; } = string.Empty;
    public string EmpGender { get; set; } = string.Empty;

    // çalışma bilgileri 
    public DateTime HireDate { get; set; }

    public int DepartmentId { get; set; }
    public Department Department { get; set; } = null!;

    public int PositionId { get; set; }
    public Position Position { get; set; } = null!;

    public int? ManagerId { get; set; }
    public Employee? Manager { get; set; }

    public bool IsActive { get; set; } = true;
    
    // inheritince kısmı 

    public ICollection<OnboardingProcess> OnboardingProcesses { get; set; } = new List<OnboardingProcess>();
}