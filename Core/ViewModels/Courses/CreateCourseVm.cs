using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Core.ViewModels.Courses;

public class CreateCourseVm:BaseCourseVm
{
    public int CenterId { get; set; }
}