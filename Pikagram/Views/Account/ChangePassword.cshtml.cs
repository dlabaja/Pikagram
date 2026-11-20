using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Pikagram.Views.Account;

// redirectne uživatele na Login
[Authorize]
public class ChangePasswordModel : PageModel
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;

    public ChangePasswordModel(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }
    
    [BindProperty]
    public InputModel Input { get; set; } = new();
    
    public class InputModel
    {
        [Required(ErrorMessage = "Aktuální heslo je povinné.")]
        public string OldPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nové heslo je povinné.")]
        [MinLength(8, ErrorMessage = "Nové heslo musí mít alespoň 8 znaků.")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Potvrzení nového hesla je povinné.")]
        [Compare(nameof(NewPassword), ErrorMessage = "Hesla se neshodují.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
    
    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Uživatel nenalezen????");
            return Page();
        }
        
        var result = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
        
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Heslo se nezměnilo");
            return Page();
        }
        
        return RedirectToPage("/");
    }
}