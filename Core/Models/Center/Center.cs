using Core.common;

namespace Core.Models.Center;

public class Center : BaseEntity
{
    public Center()
    {
        Teachers = new HashSet<Teacher.Teacher>();
        Courses = new HashSet<Course.Course>();
        Students = new HashSet<Student.Student>();
    }

    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
    public string UserId { get; set; }
    public User.User User { get; set; }
    public HashSet<Teacher.Teacher> Teachers { get; set; }
    public HashSet<Course.Course> Courses { get; set; }
    public HashSet<Student.Student> Students { get; set; }
}