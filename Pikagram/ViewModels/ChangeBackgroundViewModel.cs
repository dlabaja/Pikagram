using System.ComponentModel.DataAnnotations;

namespace Pikagram.ViewModels;

public class ChangeBackgroundViewModel
{
    [Required(ErrorMessage = "Vyber obrázek")]
    public IFormFile Media { get; set; }
}