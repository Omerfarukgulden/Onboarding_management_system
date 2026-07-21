using Onboard_management_system.OnboardingDomain.Enums;

namespace Onboard_management_system.OnboardingApplication.Dtos;

public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public int UserId { get; set; }
    public string Username {get; set;} = string.Empty;
    public UserRole Role { get; set; }
    
}