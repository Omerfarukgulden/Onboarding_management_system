using Onboard_management_system.OnboardingDomain.Enums;

namespace Onboard_management_system.OnboardingApplication.Dtos;

//user için gereken veriler 
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

//user oluşturmak için gerkeen veriler 
public class CreateUserDto
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty; // düz metin sadece burada hash'lenecek
    public UserRole Role { get; set; }
    public int? DepartmentId { get; set; }
}


// userleri güncellemek için gereken veriler 
public class UpdateUserDto
{
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public int? DepartmentId { get; set; }
    public bool IsActive { get; set; }
}