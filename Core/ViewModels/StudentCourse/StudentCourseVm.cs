namespace Core.ViewModels.StudentCourse;

public class StudentCourseVm
{
    public int  Id { get; set; }
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public double Grade { get; set; }
}