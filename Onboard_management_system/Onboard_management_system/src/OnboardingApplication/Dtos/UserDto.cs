using Onboard_management_system.OnboardingDomain.Enums;

namespace Onboard_management_system.OnboardingApplication.Dtos;

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public int? DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateUserDto
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty; // düz metin sadece burada, hemen hash'lenecek
    public UserRole Role { get; set; }
    public int? DepartmentId { get; set; }
}

public class UpdateUserDto
{
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public int? DepartmentId { get; set; }
    public bool IsActive { get; set; }
}