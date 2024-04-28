using Mapster;

namespace Core.ViewModels.Courses;

public class GetAllCoursesVm : BaseCourseVm, IRegister
{
    public int Id { get; set; }
    public int StudentCount { get; set; }
    public int TeachersCount { get; set; }

    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Core.Models.Course.Course, GetAllCoursesVm>()
            .Map(dest => dest.TeachersCount, src => src.Teachers.Count(t => t.CourseId == src.Id))
            .Map(dest => dest.StudentCount, src => src.StudentCourse.Count(sc => sc.CourseId == src.Id));
        /*Id = x.Id,
        Name = x.Name,
        Price = x.Price,
        Hours = x.Hours,
        StudentCount = x.StudentCourse.Count(sc => sc.CourseId == x.Id),
        TeachersCount = x.Teachers.Count(t => t.CourseId == x.Id)*/
    }
}