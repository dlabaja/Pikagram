using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Pikagram.ViewModels;

public class LoginViewModel : PageModel
{
    [Required(ErrorMessage = "Přezdívka je povinná.")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Heslo je povinné.")]
    public string Password { get; set; } = string.Empty;
}