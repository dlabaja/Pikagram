using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Pikagram.Views.Account;

public class RegisterModel : PageModel
{
    public RegisterModel()
    {
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

        [Required(ErrorMessage = "Potvrzení hesla je povinné.")]
        [Compare(nameof(Password), ErrorMessage = "Hesla se neshodují.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Range(typeof(bool), "true", "true", ErrorMessage = "Musíš dát své údaje našim partnerům.")]
        public bool PrivacyPolicy { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "Musíš svého syna obětovat ve jménu jazyka C.")]
        public bool SonChecked { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "Musíš mít rád/a ananas na pizze.")]
        public bool AnanasChecked { get; set; }
    }
}