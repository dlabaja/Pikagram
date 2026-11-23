using System.ComponentModel.DataAnnotations;

namespace Pikagram.ViewModels;

public class SearchViewModel
{
    [Required(ErrorMessage = "Hledaný text je povinný")]
    [StringLength(250, ErrorMessage = "Hledaný text je příliš dlouhý")]
    public string Text { get; set; } = string.Empty;
}