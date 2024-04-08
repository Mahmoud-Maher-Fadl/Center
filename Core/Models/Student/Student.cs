using Core.common;

namespace Core.Models.Student;

public class Student : BaseEntity
{
    public Student()
    {
        StudentCourses = new HashSet<StudentCourse.StudentCourse>();
    }

    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string SSN { get; set; } = string.Empty;
    public double Age { get; set; }
    public int CenterId { get; set; }
    public Center.Center Center { get; set; }
    public string UserId { get; set; }
    public User.User User { get; set; }

    public HashSet<StudentCourse.StudentCourse> StudentCourses { get; set; }
}