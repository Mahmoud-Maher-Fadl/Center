using Core.common;
using Core.Models.User;
using Infrastructure.common;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Presentation;
using Presentation.Validations;
using Presentation.Validations.Implementations;
using Presentation.Validations.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services
    .AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});
// Add services to the container.
builder.Services.AddControllersWithViews();

//builder.Services.InstallServicesInAssembly(builder.Configuration);
builder.Services.AddScoped<ICentersCrud, CentersCrud>();
builder.Services.AddScoped<ICoursesCrud, CoursesCrud>();
builder.Services.AddScoped<ITeachersCrud, TeachersCrud>();
builder.Services.AddScoped<IStudentsCrud, StudentsCrud>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//builder.Services.AddTransient<IJwtService, JwtService>();

builder.Services.AddHttpContextAccessor();
var app = builder.Build();
await app.RoleSeeds();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors("AllowAllOrigins");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();