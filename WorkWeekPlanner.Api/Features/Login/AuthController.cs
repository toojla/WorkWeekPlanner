using WorkWeekPlanner.Api.Features.Login.Models;
using WorkWeekPlanner.Api.Features.Login.Services;

namespace WorkWeekPlanner.Api.Features.Login;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        return Ok();
    }
}