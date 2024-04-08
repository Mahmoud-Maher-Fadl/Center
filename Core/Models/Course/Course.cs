using Core.common;

namespace Core.Models.Course;

public class Course : BaseEntity
{
    public Course()
    {
        Teachers = new HashSet<Teacher.Teacher>();
        StudentCourse = new HashSet<StudentCourse.StudentCourse>();
    }

    public string Name { get; set; } = string.Empty;
    public int Hours { get; set; }
    public decimal Price { get; set; }
    public int CenterId { get; set; }
    public Center.Center Center { get; set; }
    public HashSet<Teacher.Teacher> Teachers { get; set; }
    public HashSet<StudentCourse.StudentCourse> StudentCourse { get; set; }
}