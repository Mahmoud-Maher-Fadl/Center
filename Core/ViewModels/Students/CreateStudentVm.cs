using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Core.ViewModels.Students;

public class CreateStudentVm
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string SSN { get; set; } = string.Empty;
    public double Age { get; set; }
    public int CenterId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    [Compare("Password"), Display(Name="Confirm Password")]
    public string ConfirmPassword { get; set; } = string.Empty;
    public List<int> SelectedCourses { get; set; } = default!;
    public HashSet<SelectListItem> Courses { get; set; } = new HashSet<SelectListItem>();
}