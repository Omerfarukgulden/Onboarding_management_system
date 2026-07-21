using Onboard_management_system.OnboardingDomain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Onboard_management_system.OnboardingApplication.Interfaces;

namespace Onboard_management_system.OnboardingApplication.Services;

public class TokenServicee : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenServicee(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(User user, out DateTime expiresAt)
    {
        var jwtSettings = _configuration.GetSection("Jwt");

        var key = jwtSettings["Key"]
                  ?? throw new InvalidOperationException(
                      "appsettings.json içerisinde Jwt:Key bulunamadı."
                  );

        var issuer = jwtSettings["Issuer"]
                     ?? throw new InvalidOperationException(
                         "appsettings.json içerisinde Jwt:Issuer bulunamadı."
                     );

        var audience = jwtSettings["Audience"]
                       ?? throw new InvalidOperationException(
                           "appsettings.json içerisinde Jwt:Audience bulunamadı."
                       );

        var expiresInMinutes = int.Parse(
            jwtSettings["ExpiresInMinutes"] ?? "60"
        );

        expiresAt = DateTime.UtcNow.AddMinutes(expiresInMinutes);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, user.Role.ToString())
        };

        if (user.DepartmentId.HasValue)
        {
            claims.Add(
                new Claim(
                    "departmentId",
                    user.DepartmentId.Value.ToString()
                )
            );
        }

        var signingKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(key)
        );

        var credentials = new SigningCredentials(
            signingKey,
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}