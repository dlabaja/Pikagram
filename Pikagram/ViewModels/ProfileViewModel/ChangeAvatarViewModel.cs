using System.ComponentModel.DataAnnotations;

namespace Pikagram.ViewModels.ProfileViewModel;

public class ChangeAvatarViewModel
{
    [Required(ErrorMessage = "Vyber obrázek")]
    public IFormFile Media { get; set; }
}