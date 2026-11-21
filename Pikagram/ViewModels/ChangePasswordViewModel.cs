using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace Pikagram.ViewModels;

// redirectne uživatele na Login
[Authorize]
public class ChangePasswordModel
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