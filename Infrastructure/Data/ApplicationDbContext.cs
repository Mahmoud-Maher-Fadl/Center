using Core.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<IdentityRole>().ToTable("Roles");
        modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
        modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
        modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
        modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
        modelBuilder.Entity<Core.Models.Center.Center>()
            .HasQueryFilter(e => !e.IsDeleted);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Core.Models.Center.Center> Centers { get; set; }

    public DbSet<Core.Models.Course.Course> Courses { get; set; }

    public DbSet<Core.Models.Teacher.Teacher> Teachers { get; set; }
    public DbSet<Core.Models.Student.Student> Students { get; set; }
    public DbSet<Core.Models.StudentCourse.StudentCourse> StudentCourses { get; set; }
}