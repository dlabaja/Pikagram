using Microsoft.AspNetCore.Mvc;

namespace Pikagram.Controllers;

public class AccountController : Controller
{
    public IActionResult Login()
    {
        return View();
    }
    
    public IActionResult Register()
    {
        return View();
    }
    
    public IActionResult ChangePassword()
    {
        return View();
    }
}