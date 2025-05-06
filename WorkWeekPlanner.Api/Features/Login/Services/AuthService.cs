using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WorkWeekPlanner.Api.Features.Settings;

namespace WorkWeekPlanner.Api.Features.Login.Services;

public class AuthService(IUserRepository userRepository, IAppSettings appSettings) : IAuthService
{
    public async Task<(bool IsAuthenticated, string? Token)> AuthenticateAsync(string username, string password)
    {
        var user = await userRepository.GetUserByIdAsync(username);
        var claims = new List<Claim>();

        if (user is null) return (false, null);

        claims.Add(new Claim(ClaimTypes.Name, user.Username));
        claims.Add(new Claim(AppConstants.UserIdClaim, user.Id));

        if (password.Equals(user.Password))
        {
            claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role)));
        }
        else
        {
            return (false, null);
        }

        var token = GenerateToken(claims);

        return (true, token);
    }

    public string GenerateToken(IEnumerable<Claim> claims)
    {
        var key = Encoding.UTF8.GetBytes(appSettings.AppConfiguration.Key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = appSettings.AppConfiguration.Issuer,
            Audience = appSettings.AppConfiguration.Audience,
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(appSettings.AppConfiguration.ExpiryInHours),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);
        return tokenString;
    }
}