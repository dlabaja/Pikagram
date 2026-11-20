using Microsoft.AspNetCore.Mvc;

namespace Pikogram.Controllers.Api;

[ApiController]
[Route("api/account")]
public class AccountController : Controller
{
    [HttpPost("login")]
    public async Task<IActionResult> Login()
    {
        return Ok();
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register()
    {
        return Ok();
    }
    
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword()
    {
        return Ok();
    }
}