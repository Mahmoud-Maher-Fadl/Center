using Core.Models.Teacher;
using Core.ViewModels.Teachers;
using Infrastructure.common;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Teacher;

public class TeacherRepository : BaseRepository<Core.Models.Teacher.Teacher>, ITeacherRepository
{
    public TeacherRepository(ApplicationDbContext context) : base(context)
    {
    }

    public Task<List<GetAllTeachersVm>> GetAll(int? centerId)
    {
        return Context.Teachers
            .Where(x => x.CenterId == centerId)
            .Include(x => x.Center)
            .Include(x => x.Course)
            .Select(x => new GetAllTeachersVm
            {
                Id = x.Id,
                Name = x.Name,
                Salary = x.Salary,
                Age = x.Age,
                CourseName = x.Course != null ? x.Course.Name : "",
                Image = x.Image
                //    CoursesCount = x.Courses.Count
            }).ToListAsync();
    }

    public Task<Core.Models.Teacher.Teacher?> GetById(int? id)
    {
        return Context.Teachers
            .Where(x => x.Id == id)
            .Include(x => x.Course)
            .Include(x => x.Center)
            .Include(x=>x.User)
            .FirstOrDefaultAsync();
    }

    public IEnumerable<SelectListItem> GetCenterTeachersSelectList(int centerId)
    {
        return Context.Teachers
            .Where(x => x.CenterId == centerId)
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });
    }

    public Task<Core.Models.Teacher.Teacher?> GetByUserId(string userId)
    {
        return Context.Teachers
            .Where(x => x.UserId == userId)
            .Include(x => x.Course)
            .Include(x => x.Center)
            .Include(x=>x.User)
            .FirstOrDefaultAsync();
    }
}