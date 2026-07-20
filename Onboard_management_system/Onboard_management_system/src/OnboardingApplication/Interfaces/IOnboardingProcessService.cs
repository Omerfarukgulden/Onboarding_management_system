using Onboard_management_system.OnboardingApplication.Dtos;

namespace Onboard_management_system.OnboardingApplication.Interfaces;

public interface IOnboardingProcessService
{
    Task<IEnumerable<OnboardingProcessDto>> GetAllAsync();
    Task<OnboardingProcessDto?> GetByIdAsync(int id);
    Task<OnboardingProcessDto> StartAsync(StartOnboardingProcessDto dto);
}