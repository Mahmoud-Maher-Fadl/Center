using Core.ViewModels.StudentCourse;

namespace Core.ViewModels.Students;

public class GetStudentByIdVm: GetAllStudentsVm
{
    public List<StudentCourseVm> Courses { get; set; } = new List<StudentCourseVm>();
}