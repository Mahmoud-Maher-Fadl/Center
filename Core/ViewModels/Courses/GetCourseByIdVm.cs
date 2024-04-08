using Core.ViewModels.StudentCourse;

namespace Core.ViewModels.Courses;

public class GetCourseByIdVm : GetAllCoursesVm
{
    public List<StudentCourseVm> Students { get; set; } = new List<StudentCourseVm>();
    public List<TeacherCoursesVm> Teachers { get; set; } = new List<TeacherCoursesVm>();
}