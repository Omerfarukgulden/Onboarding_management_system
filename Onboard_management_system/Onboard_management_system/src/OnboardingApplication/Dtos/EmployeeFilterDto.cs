using Onboard_management_system.OnboardingApplication.Common;

namespace Onboard_management_system.OnboardingApplication.Dtos;

public class EmployeeFilterDto : PaginationParams
{
    public int? DepartmentId { get; set; }
    public int? PositionId { get; set; }
    public bool? IsActive { get; set; }
    public string? Search { get; set; }  // isim/soyisim/email içinde arama
}