using System.Linq.Expressions;
using Core.Models.StudentCourse;
using Infrastructure.common;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.StudentCourse;

public class StudentCourseRepository : BaseRepository<Core.Models.StudentCourse.StudentCourse>, IStudentCourseRepository
{
    public StudentCourseRepository(ApplicationDbContext context) : base(context)
    {
    }

    public IEnumerable<SelectListItem> GetStudentCoursesSelectList(int studentId)
    {
        var courses = Context.StudentCourses
            .Where(x => x.StudentId == studentId)
            .Include(x => x.Course)
            .Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.Course.Name
            }).ToListAsync();
        return courses.Result;
    }

    public Task<List<Core.Models.StudentCourse.StudentCourse>> GetAllFiltered(
        params Expression<Func<Core.Models.StudentCourse.StudentCourse, bool>>[] filters)
    {
        try
        {
            var query = Context.StudentCourses.AsQueryable();

            // Apply filters if provided
            query = filters.Aggregate(query, (current, filter) => current.Where(filter));
            // ==>
            /*foreach (var filter in filters)
                 query = query.Where(filter);*/
            return query.ToListAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Task.FromResult(new List<Core.Models.StudentCourse.StudentCourse>());
        }
    }
}