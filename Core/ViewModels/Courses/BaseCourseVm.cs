using System.ComponentModel.DataAnnotations;

namespace Core.ViewModels.Courses;

public class BaseCourseVm
{
    public string Name { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "Duration (Hours) must be greater than 0.")]
    public int Hours { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    public decimal Price { get; set; }
}