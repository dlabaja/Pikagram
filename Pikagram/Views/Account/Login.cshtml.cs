using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Pikagram.Views.Account;

public class LoginModel : PageModel
{
    [BindProperty]
    public InputModel Input { get; set; } = new();
    
    public class InputModel
    {
        [Required(ErrorMessage = "Email je povinný.")]
        [EmailAddress(ErrorMessage = "Zadej platný email.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Heslo je povinné.")]
        public string Password { get; set; } = string.Empty;
    }
}

