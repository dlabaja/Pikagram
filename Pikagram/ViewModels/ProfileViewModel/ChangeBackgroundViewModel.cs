using System.ComponentModel.DataAnnotations;

namespace Pikagram.ViewModels.ProfileViewModel;

public class ChangeBackgroundViewModel
{
    [Required(ErrorMessage = "Vyber obrázek")]
    public IFormFile Media { get; set; }
}