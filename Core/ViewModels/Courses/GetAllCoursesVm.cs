namespace Core.ViewModels.Courses;

public class GetAllCoursesVm:BaseCourseVm
{
    public int Id { get; set; }
    public int StudentCount { get; set; }
    public int TeachersCount { get; set; }
}