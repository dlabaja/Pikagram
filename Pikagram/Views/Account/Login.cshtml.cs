using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Pikagram.Views.Account;

public class LoginModel : PageModel
{
    private readonly SignInManager<IdentityUser> _signInManager;

    public LoginModel(SignInManager<IdentityUser> signInManager)
    {
        _signInManager = signInManager;
    }

    [BindProperty] public InputModel Input { get; set; } = new();

    public class InputModel
    {
        [Required(ErrorMessage = "Přezdívka je povinná.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Heslo je povinné.")]
        public string Password { get; set; } = string.Empty;
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        // validace selhala, běž si spravit údaje (vrátím mu stránku)
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var result = await _signInManager.PasswordSignInAsync(Input.Username, Input.Password, true, false);
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Neplatná přezdívka nebo heslo. Možná tady ani nemáš účet? Měl/a bys používat Password Manager <3");
            return Page();
        }
        
        // LocalRedirect redirectuje relativní cestu (nebo tak nějak), blokuje redirect na cizí doménu
        return returnUrl != null ? LocalRedirect(returnUrl) : RedirectToPage("/");
    }
}