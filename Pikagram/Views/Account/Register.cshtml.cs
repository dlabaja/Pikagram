using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Pikagram.Views.Account;

public class RegisterModel : PageModel
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public RegisterModel(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    // bind zajistí, že se Input naplní daty z požadavku
    [BindProperty]
    public InputModel Input { get; set; } = new();

    // model pro formulář
    public class InputModel
    {
        [Required(ErrorMessage = "Email je povinný.")]
        [EmailAddress(ErrorMessage = "Zadej platný email.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Přezdívka je povinná.")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Přezdívka musí mít 3 až 30 znaků.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Heslo je povinné.")]
        [MinLength(8, ErrorMessage = "Heslo musí mít alespoň 8 znaků.")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Heslo musíš taky potvrdit.")]
        [Compare(nameof(Password), ErrorMessage = "Hesla se neshodují.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Range(typeof(bool), "true", "true", ErrorMessage = "Musíš dát své údaje našim partnerům.")]
        public bool PrivacyPolicy { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "Musíš svého syna obětovat ve jménu jazyka C.")]
        public bool SonChecked { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "Musíš mít rád/a ananas na pizze.")]
        public bool AnanasChecked { get; set; }
    }
    
    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = new IdentityUser
        {
            Email = Input.Email,
            UserName = Input.Username
        };
        
        var result = await _userManager.CreateAsync(user, Input.Password);
        
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Registrace selhala, asi nemáte rádi ananas na pizze ;(");
            return Page();
        }
        
        await _signInManager.SignInAsync(user, true);
        return RedirectToPage("/");
    }
}