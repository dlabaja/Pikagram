using System.ComponentModel.DataAnnotations;

namespace Pikagram.Attributes;

public class ValidateDateBorn : ValidationAttribute
{
    protected override ValidationResult? IsValid(object date, ValidationContext validationContext)
    { 
        return (DateTime)date <= DateTime.UtcNow
            ? ValidationResult.Success
            : new ValidationResult(ErrorMessage); 
    }
}