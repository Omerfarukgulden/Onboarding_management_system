using Onboard_management_system.OnboardingApplication.Dtos;

namespace Onboard_management_system.OnboardingApplication.Interfaces;

public interface IPositionService
{
    Task<IEnumerable<PositionDto>> GetAllAsync();
    Task<PositionDto?> GetByIdAsync(int id);
    Task<PositionDto> CreateAsync(CreatePositionDto dto);
    Task<bool> UpdateAsync(int id, UpdatePositionDto dto);
    Task<bool> DeleteAsync(int id);
}