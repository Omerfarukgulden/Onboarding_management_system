using Onboard_management_system.OnboardingApplication.Dtos;

namespace Onboard_management_system.OnboardingApplication.Interfaces;

public interface IOnboardingTaskService
{
    Task<IEnumerable<OnboardingTaskDto>> GetAllAsync();
    Task<OnboardingTaskDto?> GetByIdAsync(int id);
    Task<bool> UpdateStatusAsync(int id, UpdateOnboardingTaskStatusDto dto);
    Task<bool> UpdateNoteAsync(int id, UpdateOnboardingTaskNoteDto dto);
    Task<IEnumerable<TaskStatusHistoryDto>> GetHistoryAsync(int taskId);
}