using System.Diagnostics;
using System.Security.Claims;
using Core.common;
using Core.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SignInManager<User> _signInManager;


    public HomeController(ILogger<HomeController> logger, IHttpContextAccessor contextAccessor, IUnitOfWork unitOfWork,
        SignInManager<User> signInManager)
    {
        _logger = logger;
        _contextAccessor = contextAccessor;
        _unitOfWork = unitOfWork;
        _signInManager = signInManager;
    }

    public IActionResult Index()
    {
        var userId = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return View();
        var student = _unitOfWork.Students.GetByUserId(userId).Result;
        if (student is not null)
            return RedirectToAction("StudentCenterCourses", "Students", new { studentId = student.Id });

        return View();
    }

    public async Task<IActionResult> Base()
    {
        var userId = _contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId is null)
            return Content("User Not Signed In");
        var user = await _signInManager.UserManager.FindByIdAsync(userId);
        if (user is null)
            return Content("User Not Signed In");
        var roles = await _signInManager.UserManager.GetRolesAsync(user);
        if (roles.Contains("Admin"))
            return RedirectToAction("Index", "Center");
        var center = await _unitOfWork.Centers.GetByUserId(user.Id);
        if (center is not null)
            return RedirectToAction("Details", "Center", new { id = center.Id });
        var teacher = await _unitOfWork.Teachers.GetByUserId(user.Id);
        if (teacher is not null)
            return RedirectToAction("Details", "Teachers", new { id = teacher.Id });
        var student = await _unitOfWork.Students.GetByUserId(user.Id);
        if (student is not null)
            return RedirectToAction("Details", "Students", new { id = student.Id });

        return Content("User Not Signed In");
    }


    [HttpPost]
    public IActionResult SelectLanguage(string culture, string returnUrl)
    {
        Response.Cookies.Append
        (
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
        );
        return LocalRedirect(returnUrl);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}