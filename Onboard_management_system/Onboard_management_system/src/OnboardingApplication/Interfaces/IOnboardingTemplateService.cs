using Onboard_management_system.OnboardingApplication.Dtos;

namespace Onboard_management_system.OnboardingApplication.Interfaces;

public interface IOnboardingTemplateService
{
    //servicede kullanılacak methodları implement ettik 
    Task<IEnumerable<OnboardingTemplateDto>> GetAllAsync();
    Task<OnboardingTemplateDto?> GetByIdAsync(int id);
    Task<OnboardingTemplateDto> CreateAsync(CreateOnboardingTemplateDto dto);
    Task<bool> UpdateAsync(int id, UpdateOnboardingTemplateDto dto);
    Task<bool> DeleteAsync(int id);
    Task<OnboardingTemplateTaskDto> AddTaskAsync(int templateId, CreateOnboardingTemplateTaskDto dto);
    Task<IEnumerable<OnboardingTemplateTaskDto>> GetTasksAsync(int templateId);
}