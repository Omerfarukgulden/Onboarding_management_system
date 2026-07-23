using Onboard_management_system.OnboardingApplication.Common;
using Onboard_management_system.OnboardingApplication.Dtos;
namespace Onboard_management_system.OnboardingApplication.Interfaces;

public interface IEmployeeService
{
    //servicede kullanılacak methodları implement ettik 
    Task<PagedResult<EmployeeDto>> GetAllAsync(EmployeeFilterDto filter);
    Task<EmployeeDto?> GetByIdAsync(int empId);
    Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto);
    Task<bool> UpdateAsync(int empId, UpdateEmployeeDto dto);
    Task<bool> DeleteAsync(int empId);
}