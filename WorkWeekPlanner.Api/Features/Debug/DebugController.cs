using Microsoft.AspNetCore.Authorization;

namespace WorkWeekPlanner.Api.Features.Debug;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DebugController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        DateTimeOffset expireDate = default;
        var expClaim = User.Claims.FirstOrDefault(c => c.Type == "exp");
        if (expClaim != null && long.TryParse(expClaim.Value, out var expValue))
        {
            expireDate = DateTimeOffset.FromUnixTimeSeconds(expValue).UtcDateTime;
        }
        return Ok(new
        {
            Message = "Debug endpoint is working",
            DateTime = DateTime.UtcNow,
            Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
            Version = Environment.Version.ToString(),
            MachineName = Environment.MachineName,
            UserName = Environment.UserName,
            TokenExpire = expireDate
        });
    }
}