using System.Security.Claims;

namespace WorkWeekPlanner.Api.Features.Login.Services;

public interface IAuthService
{
    Task<(bool IsAuthenticated, string? Token)> AuthenticateAsync(string username, string password);

    string GenerateToken(IEnumerable<Claim> claims);
}