using Infrastructure.common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Teacher;

public class TeacherConfiguration : BaseConfiguration<Core.Models.Teacher.Teacher>
{
    protected override void Configure(EntityTypeBuilder<Core.Models.Teacher.Teacher> builder, string tableName)
    {
        builder.HasOne(x => x.Center)
            .WithMany(c => c.Teachers)
            .HasForeignKey(x => x.CenterId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x => x.Course)
            .WithMany(i => i.Teachers)
            .HasForeignKey(x => x.CourseId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(s => s.User)
            .WithOne()
            .HasForeignKey<Core.Models.Teacher.Teacher>(s => s.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}