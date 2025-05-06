namespace WorkWeekPlanner.Api.Features.Login;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel? model)
    {
        if (model is null) return BadRequest("Invalid client request");

        var result = await authService.AuthenticateAsync(model.UserName, model.Password);

        if (result.IsAuthenticated)
        {
            return Ok(new { result.Token });
        }
        return Unauthorized("Invalid username or password");
    }
}