using System.Linq.Expressions;
using Core.common;
using Core.ViewModels.Courses;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Core.Models.Course;

public interface ICourseRepository : IBaseRepository<Course>
{
    Task<List<GetAllCoursesVm>> GetAll(int? centerId = 0);
    Task<Course?> GetById(int id);
    IEnumerable<SelectListItem> GetCenterCoursesSelectList(int centerId);
    Task<List<Course>> GetAllFiltered(
        params Expression<Func<Course, bool>>[] filters);
}