using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace Core.ViewModels.Students;

public class UpdateStudentVm
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Ssn { get; set; } = string.Empty;
    public double Age { get; set; }

    [Display(Name = "Center")] public int CenterId { get; set; }

    //public IEnumerable<SelectListItem> Centers { get; set; } = Enumerable.Empty<SelectListItem>();
    public List<int>? SelectedCourses { get; set; } = new List<int>();
    public IEnumerable<SelectListItem>? Courses { get; set; } = Enumerable.Empty<SelectListItem>();
}