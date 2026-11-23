using System.ComponentModel.DataAnnotations;

namespace Pikagram.ViewModels;

public class ChangeBioViewModel
{
    [Required(ErrorMessage = "Bio je povinné")]
    [StringLength(250, ErrorMessage = "Bio je příliš dlouhé")]
    public string Bio { get; set; } = string.Empty;
}