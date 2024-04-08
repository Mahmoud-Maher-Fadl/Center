using System.ComponentModel.DataAnnotations;
using Core.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Presentation.Attributes;

namespace Core.ViewModels.Teachers;

public class CreateTeacherVm : BaseTeacherVm
{
    //public string? Image { get; set; }
    [Display(Name = "Center")] public int CenterId { get; set; }
    [Display(Name = "Course")] public int? CourseId { get; set; }
    public IEnumerable<SelectListItem> Courses { get; set; } = Enumerable.Empty<SelectListItem>();

    [AllowedExtension(FileSettings.AllowedExtension), MaxFileSize(FileSettings.MaxFileSizeInBytes)]
    public IFormFile? Image { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    [Compare("Password"), Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; } = string.Empty;
}