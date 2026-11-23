using System.ComponentModel.DataAnnotations;

namespace Pikagram.ViewModels;

public class SendMessageViewModel
{
    [Required(ErrorMessage = "Zpráva je povinná")]
    [StringLength(250, ErrorMessage = "Zpráva je příliš dlouhá")]
    public string Message { get; set; } = string.Empty;
}