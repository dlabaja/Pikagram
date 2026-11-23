using System.ComponentModel.DataAnnotations;

namespace Pikagram.ViewModels;

public class SendMessageViewModel
{
    [Required(ErrorMessage = "Zpráva je povinný")]
    [StringLength(250, ErrorMessage = "Zpráva je příliš dlouhá")]
    public string Message { get; set; } = string.Empty;
}