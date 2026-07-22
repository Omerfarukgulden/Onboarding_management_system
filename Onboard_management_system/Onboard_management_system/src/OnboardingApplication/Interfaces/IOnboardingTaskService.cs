using Onboard_management_system.OnboardingApplication.Dtos;

namespace Onboard_management_system.OnboardingApplication.Interfaces;

public interface IOnboardingTaskService
{
    //servicede kullanılacak methodları implement ettik 
    Task<IEnumerable<OnboardingTaskDto>> GetAllAsync();
    Task<OnboardingTaskDto?> GetByIdAsync(int id);
    Task<bool> UpdateStatusAsync(int id, UpdateOnboardingTaskStatusDto dto, int currentUserId, string currentUserRole, int? currentUserDepartmentId);
    Task<bool> UpdateNoteAsync(int id, UpdateOnboardingTaskNoteDto dto);
    Task<IEnumerable<TaskStatusHistoryDto>> GetHistoryAsync(int taskId);
}