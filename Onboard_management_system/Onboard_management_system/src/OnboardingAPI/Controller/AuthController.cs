using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Onboard_management_system.OnboardingApplication.Dtos;
using Onboard_management_system.OnboardingApplication.Interfaces;
using Onboard_management_system.OnboardingInfrastructure.Context;

namespace Onboard_management_system.OnboardingAPI.Controller;


[ApiController ]
[Route("api/[controller]/[action]")]
public class AuthController : ControllerBase
{
    private readonly OnboardingDbContext _context;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthController> _logger;
   

    public AuthController(OnboardingDbContext context, ITokenService tokenService , ILogger<AuthController> logger)
    {
        _context = context;
        _tokenService = tokenService;
        _logger = logger;
    }

    [HttpPost]//("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username && u.IsActive);

        if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
            _logger.LogWarning("Başarısız giriş denemesi. Kullanıcı adı: {Username}", dto.Username); 
            return Unauthorized("kullanıcı adi veya şifre hatalı");
        }

        var token = _tokenService.GenerateToken(user, out var expiresAt);
        
        _logger.LogInformation("Kullanıcı giriş yaptı. UserId: {UserId}, Username: {Username}, Rol: {Role}",  // ← GÜNCELLENDİ
            user.Id, user.Username, user.Role);
        
        return Ok(new LoginResponseDto()
            { Token = token,
                ExpiresAt = expiresAt,
                UserId = user.Id,
                Username = user.Username,
                Role = user.Role });
    }
}