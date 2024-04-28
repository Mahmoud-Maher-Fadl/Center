using Core.Models.Student;
using Mapster;

namespace Core.ViewModels.Students;

public class GetAllStudentsVm : IRegister
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string SSN { get; set; } = string.Empty;
    public double Age { get; set; }
    public int CenterId { get; set; }
    public string CenterName { get; set; } = string.Empty;
    public int CoursesCount { get; set; }

    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Student, GetAllStudentsVm>()
            .Map(dest => dest.CenterName, src => src.Center.Name)
            .Map(dest => dest.CoursesCount, src => src.StudentCourses.Count);
        /*Id = x.Id,
        Name = x.Name,
        Address = x.Address,
        Age = x.Age,
        SSN = x.SSN,
        CenterName = x.Center.Name,
        CoursesCount = x.StudentCourses.Count*/
    }
}