namespace Core.ViewModels.Teachers;

public class GetAllTeachersVm : BaseTeacherVm
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public string? CourseName { get; set; } = string.Empty;
    public int CenterId { get; set; }
    public string? Image { get; set; } = string.Empty;
}