using System.ComponentModel.DataAnnotations;

namespace Pikagram.ViewModels;

public class SearchViewModel
{
    [Required(ErrorMessage = "Hledání je povinné")]
    [StringLength(250, ErrorMessage = "Text v hledání je příliš dlouhý")]
    public string Text { get; set; } = string.Empty;
}