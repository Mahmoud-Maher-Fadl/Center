using Core.common;
using Core.Models.Center;
using Core.Models.Course;
using Core.Models.Student;
using Core.Models.StudentCourse;
using Core.Models.Teacher;
using Infrastructure.Center;
using Infrastructure.Course;
using Infrastructure.Data;
using Infrastructure.Student;
using Infrastructure.StudentCourse;
using Infrastructure.Teacher;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.common;

public class UnitOfWork : IUnitOfWork, IAsyncDisposable
{
    private readonly ApplicationDbContext _context;
    public ICenterRepository Centers { get; private set; }

    public ITeacherRepository Teachers { get; private set; }

    //public IBaseRepository<Core.Models.Course.Course> Courses { get; private set; }
    public ICourseRepository Courses { get; private set; }

    // public IBaseRepository<Core.Models.Student.Student> Stud   public IStudentRepository Students { get; private set; }
    public IStudentRepository Students { get; private set; }

    // public IBaseRepository<Core.Models.StudentCourse.StudentCourse> StudentCourses { get; private set; }
    public IStudentCourseRepository StudentCourses { get; private set; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Centers = new CenterRepository(_context);
        Teachers = new TeacherRepository(_context);
        Courses = new CourseRepository(_context);
        Students = new StudentRepository(_context);
        // StudentCourses = new BaseRepository<Core.Models.StudentCourse.StudentCourse>(_context);
        StudentCourses = new StudentCourseRepository(_context);
    }

    public async Task<int> Complete()
    {
        var effected=await _context.SaveChangesAsync();
        return effected;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}