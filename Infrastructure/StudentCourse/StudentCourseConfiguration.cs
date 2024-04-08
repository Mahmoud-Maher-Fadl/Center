using Infrastructure.common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.StudentCourse;

public class StudentCourseConfiguration : BaseConfiguration<Core.Models.StudentCourse.StudentCourse>
{
    protected override void Configure(EntityTypeBuilder<Core.Models.StudentCourse.StudentCourse> builder, string tableName)
    {
        builder.HasOne(sc => sc.Student)
            .WithMany(s => s.StudentCourses)
            .HasForeignKey(sc => sc.StudentId)
            .OnDelete(DeleteBehavior.ClientCascade);
        builder.HasOne(sc => sc.Course)
            .WithMany(s => s.StudentCourse)
            .HasForeignKey(sc => sc.CourseId)
            .OnDelete(DeleteBehavior.ClientCascade);
    }
}