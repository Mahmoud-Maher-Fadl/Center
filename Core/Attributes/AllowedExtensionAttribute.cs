using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.Linq;
namespace Core.Attributes;

public class AllowedExtensionAttribute : ValidationAttribute
{
    private readonly string _allowedExtensions;

    public AllowedExtensionAttribute(string allowedExtensions)
    {
        _allowedExtensions = allowedExtensions;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is IFormFile file)
        {
            var fileExtension = Path.GetExtension(file.FileName);
            var isAllowed = _allowedExtensions.Split(',').Contains(fileExtension, StringComparer.OrdinalIgnoreCase);
            if (!isAllowed) 
                return new ValidationResult($"Only {_allowedExtensions} are Allowed");
        }
        return ValidationResult.Success;
    }
}