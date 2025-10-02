using Microsoft.AspNetCore.Mvc;

namespace TaskManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login()
    {
        return Ok(new { token = "placeholder", refreshToken = "placeholder" });
    }

    [HttpPost("refresh")]
    public IActionResult Refresh()
    {
        return Ok(new { token = "placeholder", refreshToken = "placeholder" });
    }
}
