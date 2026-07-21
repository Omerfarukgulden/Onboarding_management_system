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
   

    public AuthController(OnboardingDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == dto.Username && u.IsActive);

        if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
            return Unauthorized("kullanıcı adi veya şifre hatalı");
        }

        var token = _tokenService.GenerateToken(user, out var expiresAt);
        return Ok(new LoginResponseDto()
            { Token = token,
                ExpiresAt = expiresAt,
                UserId = user.Id,
                Username = user.Username,
                Role = user.Role });
    }
}