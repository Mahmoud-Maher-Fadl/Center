using System.ComponentModel.DataAnnotations;

namespace Core.ViewModels.Center;

public class BaseCenterVm
{
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Location { get; set; } = string.Empty;
}