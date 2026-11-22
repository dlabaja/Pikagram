using System.ComponentModel.DataAnnotations;

namespace Pikagram.ViewModels;

public class EditPostViewModel
{
    [StringLength(250, ErrorMessage = "Popisek je příliš dlouhý")]
    public string? Text { get; set; } = string.Empty;

    [StringLength(250, MinimumLength = 0, ErrorMessage = "Tagy jsou příliš dlouhé")]
    public string? Tags { get; set; } = string.Empty;

    public IFormFile? Media { get; set; }
}