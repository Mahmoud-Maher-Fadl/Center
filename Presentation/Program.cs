using System.Globalization;
using Core.common;
using Core.Models.User;
using Infrastructure.common;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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


#region Localization Services

builder.Services.AddLocalization(opt => { opt.ResourcesPath = ""; });

builder.Services.AddMvc()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new List<CultureInfo>
    {
        new CultureInfo("en-US"),
        new CultureInfo("ar-EG"),
    };
    options.DefaultRequestCulture = new RequestCulture( "en-US");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

#endregion


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
    app.UseHsts();
}

app.UseCors("AllowAllOrigins");
app.UseHttpsRedirection();
#region Localization Middleware

var options = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(options!.Value);
#endregion


app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();