using Onboard_management_system.OnboardingDomain.Entities;

namespace Onboard_management_system.OnboardingApplication.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user, out DateTime expiresAt);

}