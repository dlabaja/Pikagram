using System.ComponentModel.DataAnnotations;

namespace Pikagram.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Přezdívka je povinná.")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Heslo je povinné.")]
    public string Password { get; set; } = string.Empty;
}