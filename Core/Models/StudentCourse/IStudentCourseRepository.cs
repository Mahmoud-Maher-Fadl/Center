using System.Linq.Expressions;
using Core.common;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Core.Models.StudentCourse;

public interface IStudentCourseRepository : IBaseRepository<StudentCourse>
{
    
    IEnumerable<SelectListItem> GetStudentCoursesSelectList(int studentId);
    

    Task<List<StudentCourse>> GetAllFiltered(
        params Expression<Func<StudentCourse, bool>>[] filters);
}