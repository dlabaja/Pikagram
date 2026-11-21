using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Pikagram.ViewModels;

public class RegisterViewModel : PageModel
{
    [Required(ErrorMessage = "Jméno je povinné (Ale můžeš si ho vymyslet).")]
    [StringLength(50, ErrorMessage = "Jméno může mít maximálně 50 znaků.")]
    public string FirstName { get; set; } = string.Empty;
        
    [Required(ErrorMessage = "Příjmení je povinné (Ale můžeš si ho vymyslet).")]
    [StringLength(50, ErrorMessage = "Příjmení může mít maximálně 50 znaků.")]
    public string LastName { get; set; } = string.Empty;
        
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