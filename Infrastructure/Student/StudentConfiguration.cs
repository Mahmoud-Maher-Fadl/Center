using Infrastructure.common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Student;

public class StudentConfiguration : BaseConfiguration<Core.Models.Student.Student>
{
    protected override void Configure(EntityTypeBuilder<Core.Models.Student.Student> builder, string tableName)
    {
        builder.HasIndex(x => x.SSN).IsUnique();
        builder.Property(x => x.SSN)
            .HasMaxLength(14);
        builder.HasOne(x => x.Center)
            .WithMany(c => c.Students)
            .HasForeignKey(x => x.CenterId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(s => s.User)
            .WithOne()
            .HasForeignKey<Core.Models.Student.Student>(s => s.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}