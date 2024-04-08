using Core.common;

namespace Core.Models.Teacher;

public class Teacher : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public double Age { get; set; }
    public string? Image { get; set; } = string.Empty;
    public int CenterId { get; set; }
    public Center.Center Center { get; set; } //cascade
    public int? CourseId { get; set; }
    public Course.Course? Course { get; set; }
    public string UserId { get; set; }
    public User.User User { get; set; }
}