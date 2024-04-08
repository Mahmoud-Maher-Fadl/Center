using Microsoft.AspNetCore.Mvc.Rendering;

namespace Core.ViewModels.Students;

public class StudentCenterCourses
{
    public int StudentId { get; set; }
    public List<SelectListItem> Courses { get; set; } = new List<SelectListItem>();

    public List<int> SelectedCourses { get; set; } = new List<int>();
}