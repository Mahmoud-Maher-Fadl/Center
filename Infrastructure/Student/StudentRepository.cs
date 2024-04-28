using Core.Models.Student;
using Core.ViewModels.Students;
using Infrastructure.common;
using Infrastructure.Data;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Student;

public class StudentRepository : BaseRepository<Core.Models.Student.Student>, IStudentRepository
{
    public StudentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public Task<List<GetAllStudentsVm>> GetAll(int? id)
    {
        return Context.Students
            .Where(x => x.CenterId == id)
            .Include(x => x.StudentCourses)
            .ProjectToType<GetAllStudentsVm>()
            .ToListAsync();
            /*.Select(x => new GetAllStudentsVm()
            {
                Id = x.Id,
                Name = x.Name,
                Address = x.Address,
                Age = x.Age,
                SSN = x.SSN,
                CenterName = x.Center.Name,
                CoursesCount = x.StudentCourses.Count
            })
            .ToListAsync();*/
    }

    public async Task<Core.Models.Student.Student?> GetById(int id)
    {
        var student = await Context.Students
            .Where(x => x.Id == id)
            .Include(x => x.StudentCourses)
            .ThenInclude(x => x.Course)
            .FirstOrDefaultAsync();
        return student;
    }

    public async Task<Core.Models.Student.Student?> GetByUserId(string userId)
    {
        var student = await Context.Students
            .Include(x=>x.User)
            .FirstOrDefaultAsync(x => x.UserId == userId);
        return student;
    }
}