using Core.common;
using Core.ViewModels.Courses;
using Core.ViewModels.StudentCourse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Validations.Interfaces;

namespace Presentation.Controllers;

[Authorize(Roles = "Center")]
public class CoursesController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICoursesCrud _coursesCrud;

    public CoursesController(IUnitOfWork unitOfWork, ICoursesCrud coursesCrud)
    {
        _unitOfWork = unitOfWork;
        _coursesCrud = coursesCrud;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult GetAll()
    {
        if (!int.TryParse(Request.Cookies["centerId"], out var centerId))
            return Content("No Logged In Center");
        var courses = _unitOfWork.Courses.GetAll(centerId).Result;
        return Json(courses);
    }

    public IActionResult Details(int id)
    {
        var course = _unitOfWork.Courses.GetById(id).Result;
        if (course is null)
            return Content("Course Not Found");
        var students = _unitOfWork.StudentCourses
            .GetAllFiltered(x => x.CourseId == course.Id)
            .Result.Select(x => new StudentCourseVm()
            {
                StudentId = x.StudentId,
                StudentName = x.Student.Name,
                CourseId = x.CourseId,
                CourseName = x.Course.Name,
                Grade = x.Grade
            }).ToList();
        var courseVm = new GetCourseByIdVm()
        {
            Id = course.Id,
            Name = course.Name,
            Hours = course.Hours,
            Price = course.Price,
            StudentCount = course.StudentCourse.Count,
            TeachersCount = course.Teachers.Count,
            Students = students,
            Teachers = course.Teachers
                .Select(x => new TeacherCoursesVm()
                {
                    TeacherId = x.Id,
                    TeacherName = x.Name
                }).ToList()
        };
        return View(courseVm);
    }

    [HttpGet]
    public IActionResult Create(CreateCourseVm course)
    {
        return View(course);
    }

    [HttpPost]
    public async Task<IActionResult> SaveCreate(CreateCourseVm courseVm)
    {
        if (!int.TryParse(Request.Cookies["centerId"], out var centerId))
            return Content("No Logged In Center");
        courseVm.CenterId = centerId;
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            var message = string.Join(", ", errors);
            return Json(new { IsSuccess = false, Message = message });
        }

        var result = await _coursesCrud.Create(courseVm);
        return Json(new { result.IsSuccess, result.Message });
    }

    [HttpGet]
    public IActionResult Update(int id)
    {
        var course = _unitOfWork.Courses.GetById(id).Result;
        if (course is null)
            return Content("Course Not Found");
        TempData["courseId"] = id;
        var courseVm = new UpdateCourseVm()
        {
            Name = course.Name,
            Hours = course.Hours,
            Price = course.Price,
            CenterId = course.CenterId,
            Centers = _unitOfWork.Centers.GetCentersSelectList()
        };
        return View(courseVm);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateCourseVm courseVm)
    {
        /*
        if (!int.TryParse(Request.Cookies["centerId"], out var centerId))
            return Json(new { IsSuccess = false, Message = "No Logged In Center" });

        */
        if (TempData["courseId"] is not int courseId)
            return Json(new { IsSuccess = false, Message = "Invalid Course Id" });

        courseVm.Id = courseId;
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(x => x.ErrorMessage);
            var message = string.Join(',', errors);
            return Json(new { IsSuccess = false, Message = message });
        }

        var result = await _coursesCrud.Update(courseVm);
        return Json(new { result.IsSuccess, result.Message });
    }

    public IActionResult Delete(int id)
    {
        var result = _coursesCrud.Delete(id).Result;
        return result.IsSuccess
            ? RedirectToAction("Index", "Courses")
            : Content(result.Message);
    }
}