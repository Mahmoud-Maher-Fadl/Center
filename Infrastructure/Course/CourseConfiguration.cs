using Infrastructure.common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Course;

public class CourseConfiguration : BaseConfiguration<Core.Models.Course.Course>
{
    protected override void Configure(EntityTypeBuilder<Core.Models.Course.Course> builder, string tableName)
    {
       
        builder.HasOne(x => x.Center)
            .WithMany(c => c.Courses)
            .HasForeignKey(x => x.CenterId)
            .OnDelete(DeleteBehavior.Cascade);
       
    }
}