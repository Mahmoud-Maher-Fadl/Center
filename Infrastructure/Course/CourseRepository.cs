using System.Linq.Expressions;
using Core.Models.Course;
using Core.ViewModels.Courses;
using Core.ViewModels.StudentCourse;
using Infrastructure.common;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Course;

public class CourseRepository : BaseRepository<Core.Models.Course.Course>, ICourseRepository
{
    public CourseRepository(ApplicationDbContext context) : base(context)
    {
    }

    public Task<List<GetAllCoursesVm>> GetAll(int? id)
    {
        var courses = Context.Courses
            .Where(x => x.CenterId == id)
            .Include(x => x.Center)
            .Include(x => x.Teachers)
            .Include(x => x.StudentCourse)
            .Select(x => new GetAllCoursesVm
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                Hours = x.Hours,
                StudentCount = x.StudentCourse.Count(sc => sc.CourseId == x.Id),
                TeachersCount = x.Teachers.Count(t => t.CourseId == x.Id)
            }).ToListAsync();
        return courses;
    }

    public Task<Core.Models.Course.Course?> GetById(int id)
    {
        var course = Context.Courses
            .Where(x => x.Id == id)
            .Include(x => x.Center)
            .Include(x => x.Teachers)
            .Include(x => x.StudentCourse)
            .ThenInclude(x => x.Student)
            .FirstOrDefaultAsync();
        return course;
    }

    public IEnumerable<SelectListItem> GetCenterCoursesSelectList(int centerId)
    {
        var courses = Context.Courses
            .Where(x => x.CenterId == centerId)
            .Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToListAsync();
        return courses.Result;
    }

    public Task<List<Core.Models.Course.Course>> GetAllFiltered(
        params Expression<Func<Core.Models.Course.Course, bool>>[] filters)
    {
        try
        {
            var query = Context.Courses.AsQueryable();

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
            return Task.FromResult(new List<Core.Models.Course.Course>());
        }
    }
}