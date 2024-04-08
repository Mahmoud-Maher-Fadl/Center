using Core.Models.Center;
using Core.Models.Course;
using Core.Models.Student;
using Core.Models.StudentCourse;
using Core.Models.Teacher;
using Microsoft.EntityFrameworkCore.Storage;

namespace Core.common;

public interface IUnitOfWork : IDisposable
{
    ICenterRepository Centers { get; }

    ITeacherRepository Teachers { get; }

    ICourseRepository Courses { get; }

    // IBaseRepository<Course> Courses { get; }
    IStudentRepository Students { get; }

    //IBaseRepository<Student> Students { get; }
    IStudentCourseRepository StudentCourses { get; }

    // IBaseRepository<StudentCourse> StudentCourses { get; }

    Task<int> Complete();
    Task<IDbContextTransaction> BeginTransactionAsync();
}