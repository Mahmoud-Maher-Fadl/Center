using Core.common;

namespace Core.Models.StudentCourse;

public class StudentCourse : BaseEntity
{
    public int StudentId { get; set; }
    public Student.Student Student { get; set; }
    public int CourseId { get; set; }
    public Course.Course Course { get; set; }
    public double Grade { get; set; }
}