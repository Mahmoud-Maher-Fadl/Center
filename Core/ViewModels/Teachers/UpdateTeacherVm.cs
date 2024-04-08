using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Core.ViewModels.Teachers;

public class UpdateTeacherVm : BaseTeacherVm
{
    public int Id { get; set; }

    public string? Image { get; set; }
    public string? CurrentCover { get; set; } = string.Empty;

    [Display(Name = "Center")] public int CenterId { get; set; }
    // public IEnumerable<SelectListItem> Centers { get; set; } = Enumerable.Empty<SelectListItem>();

    [Display(Name = "Course")] public int? CourseId { get; set; }
    public IEnumerable<SelectListItem> Courses { get; set; } = Enumerable.Empty<SelectListItem>();

    /*
    [FileExtensions(Extensions = ".jpg,.png")]
    */
    public IFormFile? Cover { get; set; }
}