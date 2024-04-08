using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Core.ViewModels.Courses;

public class UpdateCourseVm : BaseCourseVm
{
    public int Id { get; set; }
    [Display(Name = "Center")] public int CenterId { get; set; }
    public IEnumerable<SelectListItem> Centers { get; set; } = Enumerable.Empty<SelectListItem>();
}