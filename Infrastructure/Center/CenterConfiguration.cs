using Infrastructure.common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Center;

public class CenterConfiguration : BaseConfiguration<Core.Models.Center.Center>
{
    protected override void Configure(EntityTypeBuilder<Core.Models.Center.Center> builder, string tableName)
    {
        builder.HasOne(s => s.User)
            .WithOne()
            .HasForeignKey<Core.Models.Center.Center>(s => s.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}