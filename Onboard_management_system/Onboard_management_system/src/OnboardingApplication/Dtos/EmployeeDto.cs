namespace Onboard_management_system.OnboardingApplication.Dtos;

public class EmployeeDto
{
    public int EmpId { get; set; }
    public string EmpName { get; set; } = string.Empty;
    public string EmpSurname { get; set; } = string.Empty;
    public string EmpEmail { get; set; } = string.Empty;
    public string EmpPhone { get; set; } = string.Empty;
    public string EmpAddress { get; set; } = string.Empty;
    public string EmpBlood { get; set; } = string.Empty;
    public string EmpGender { get; set; } = string.Empty;
    public DateTime HireDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    public int PositionId { get; set; }
    public string? PositionName { get; set; }
}

public class CreateEmployeeDto
{
    public string EmpName { get; set; } = string.Empty;
    public string EmpSurname { get; set; } = string.Empty;
    public string EmpEmail { get; set; } = string.Empty;
    public string EmpPhone { get; set; } = string.Empty;
    public string EmpAddress { get; set; } = string.Empty;
    public string EmpBlood { get; set; } = string.Empty;
    public string EmpGender { get; set; } = string.Empty;
    public DateTime HireDate { get; set; }
    public int DepartmentId { get; set; }
    public int PositionId { get; set; }
}

public class UpdateEmployeeDto : CreateEmployeeDto
{
    public DateTime? EndDate { get; set; }
}