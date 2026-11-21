using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pikagram.Models;
using Pikagram.ViewModels;

namespace Pikagram.Controllers;

public class AccountController(SignInManager<User> signInManager, UserManager<User> userManager) : Controller
{
    public IActionResult Login()
    {
        return View(new LoginViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await signInManager.PasswordSignInAsync(model.Username, model.Password, true, false);
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Neplatná přezdívka nebo heslo. Možná tady ani nemáš účet? Měl/a bys používat Password Manager <3");
            return View(model);
        }
        
        // LocalRedirect redirectuje relativní cestu (nebo tak nějak), blokuje redirect na cizí doménu
        return returnUrl != null ? LocalRedirect(returnUrl) : RedirectToPage("/");
    }
    
    public IActionResult Register()
    {
        return View(new RegisterViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model, string? returnUrl)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = new User
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            UserName = model.Username,
        };

        var result = await userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Registrace selhala, asi nemáte rádi ananas na pizze ;(");
            return View(model);
        }

        await signInManager.SignInAsync(user, true);
        return returnUrl != null ? LocalRedirect(returnUrl) : RedirectToPage("/");
    }
    
    public IActionResult ChangePassword()
    {
        return View(new ChangePasswordModel());
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Uživatel nenalezen????");
            return View(model);
        }
        
        var result = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
        
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Heslo se nezměnilo");
        }

        // tady znásilňuju metodu pro chyby, uvidím co to udělá
        ModelState.AddModelError(string.Empty, "Heslo bylo úspěšně změněno");
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToPage("/");
    }
}