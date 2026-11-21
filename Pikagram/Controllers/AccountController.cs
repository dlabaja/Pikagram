using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pikagram.Models;

namespace Pikagram.Controllers;

public class AccountController(SignInManager<User> signInManager) : Controller
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

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToPage("/");
    }
}