using Core.common;
using Core.Models.StudentCourse;
using Core.ViewModels.StudentCourse;
using Core.ViewModels.Students;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Presentation.Validations.Interfaces;

namespace Presentation.Controllers;

[Authorize(Roles = "Center,Student")]
public class StudentsController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStudentsCrud _studentsCrud;

    public StudentsController(IUnitOfWork unitOfWork, IStudentsCrud studentsCrud)
    {
        _unitOfWork = unitOfWork;
        _studentsCrud = studentsCrud;
    }


    [HttpGet]
    [Authorize(Roles = "Center")]
    public async Task<IActionResult> Index()
    {
        if (!int.TryParse(Request.Cookies["centerId"], out var centerId))
            return Content("No Logged In Center");
        var students = await _unitOfWork.Students.GetAll(centerId);
        return View(students);
    }
    /*public IActionResult Index()
    {
        return View();
    }*/

    public async Task<IActionResult> GetAll()
    {
        if (!int.TryParse(Request.Cookies["centerId"], out var centerId))
            return Content("No Logged In Center");
        var students = await _unitOfWork.Students.GetAll(centerId);
        return Json(students);
    }

    public IActionResult Details(int id)
    {
        var student = _unitOfWork.Students.GetById(id).Result;
        if (student is null)
            return Content("Student Not Found");
        var studentVm = new GetStudentByIdVm
        {
            Name = student.Name,
            Address = student.Address,
            SSN = student.SSN,
            Age = student.Age,
            Courses = student.StudentCourses
                .Select(x => new StudentCourseVm()
                {
                    StudentId = x.StudentId,
                    StudentName = x.Student.Name,
                    CourseId = x.CourseId,
                    CourseName = x.Course.Name,
                    Grade = x.Grade
                }).ToList()
        };
        return View(studentVm);
    }

    [HttpGet]
    [Authorize(Roles = "Center")]
    public IActionResult Create(CreateStudentVm studentVm)
    {
        if (!int.TryParse(Request.Cookies["centerId"], out var centerId))
            return Content("No Logged In Center");
        studentVm.Courses = _unitOfWork.Courses.GetCenterCoursesSelectList(centerId).ToHashSet();
        return View(studentVm);
    }

    [HttpPost]
    [Authorize(Roles = "Center")]
    public async Task<IActionResult> SaveCreate(CreateStudentVm studentVm)
    {
        if (!int.TryParse(Request.Cookies["centerId"], out var centerId))
            return Json(new { IsSuccess = false, Message = "No Logged In Center" });
        studentVm.CenterId = centerId;
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            var message = string.Join(", ", errors);
            return Json(new { IsSuccess = false, Message = message });
        }

        var result = await _studentsCrud.Create(studentVm);
        return Json(new { result.IsSuccess, result.Message });
    }

    [HttpGet]
    [Authorize(Roles = "Center")]
    public IActionResult Update(int id)
    {
        var student = _unitOfWork.Students.GetById(id).Result;
        if (student is null)
            return Content("Student Not Found");
        TempData["studentId"] = id;
        var studentVm = new UpdateStudentVm()
        {
            Name = student.Name,
            Address = student.Address,
            Ssn = student.SSN,
            Age = student.Age,
            CenterId = student.CenterId,
            SelectedCourses = student.StudentCourses.Select(x => x.CourseId).ToList(),
            Courses = _unitOfWork.Courses.GetCenterCoursesSelectList(student.CenterId).ToHashSet(),
        };
        return View(studentVm);
    }

    [HttpPost]
    [Authorize(Roles = "Center")]
    public async Task<IActionResult> Update(UpdateStudentVm studentVm)
    {
        if (TempData["studentId"] is not int studentId)
            return Json(new { IsSuccess = false, Message = "Invalid Student Id" });
        studentVm.Id = studentId;
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(x => x.ErrorMessage);
            var message = string.Join(',', errors);
            return Json(new { IsSuccess = false, Message = message });
        }

        var result = await _studentsCrud.Update(studentVm);
        return Json(new { result.IsSuccess, result.Message });
        /*if (ModelState.IsValid)
        {
            var result = await _studentsCrud.Update(studentVm);
            if (result.IsSuccess)
                return RedirectToAction("Index", "Students");
            return Content(result.Message);
        }

        studentVm.Courses = _unitOfWork.Courses.GetCenterCoursesSelectList(studentVm.CenterId).ToHashSet();
        return View("Update", studentVm);*/
    }

    [HttpGet]
    [Authorize(Roles = "Center")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _studentsCrud.Delete(id);
        return result.IsSuccess
            ? RedirectToAction("Index", "Students")
            : Content(result.Message);
    }

    [HttpGet]
    public async Task<IActionResult> StudentCenterCourses(int studentId)
    {
        var student = await _unitOfWork.Students.GetById(studentId);
        if (student is null)
            return Content("Student Not Found");
        var studentCourses = await _unitOfWork.StudentCourses
            .GetAllFiltered(x => x.StudentId == studentId);
        var courses = await _unitOfWork.Courses
            .GetAllFiltered(x => x.CenterId == student.CenterId);
        var studentCenterCourses = new StudentCenterCourses
        {
            StudentId = studentId,
            Courses = courses.Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.Name,
                Selected = studentCourses.Select(sc => sc.CourseId).Contains(x.Id)
            }).ToList(),
            SelectedCourses = studentCourses.Select(x => x.CourseId).ToList()
        };
        return View(studentCenterCourses);
    }

    [HttpPost]
    public async Task<IActionResult> SetStudentCourses(StudentCenterCourses studentCenterCourses)
    {
        var student = await _unitOfWork.Students.GetById(studentCenterCourses.StudentId);
        if (student is null)
            return Content("Student Not Found");
        student.StudentCourses = studentCenterCourses.SelectedCourses
            .Select(x => new StudentCourse()
            {
                StudentId = student.Id,
                CourseId = x
            }).ToHashSet();
        await _unitOfWork.Students.Update(student);
        await _unitOfWork.Complete();
        return RedirectToAction("Details", "Students", new { id = student.Id });
    }
}