using System.ComponentModel.DataAnnotations;

namespace Pikagram.Attributes;

public class MustBeTrueAttribute : ValidationAttribute
{
    public override bool IsValid(object? value) => value is true;
}