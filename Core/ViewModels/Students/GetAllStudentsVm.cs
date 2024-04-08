using Core.ViewModels.StudentCourse;

namespace Core.ViewModels.Students;

public class GetAllStudentsVm
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string SSN { get; set; } = string.Empty;
    public double Age { get; set; }
    public int CenterId { get; set; }
    public string CenterName { get; set; }= string.Empty;
    public int CoursesCount { get; set; }
}