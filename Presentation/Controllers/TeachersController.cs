using Core.common;
using Core.ViewModels.StudentCourse;
using Core.ViewModels.Teachers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Validations.Interfaces;

namespace Presentation.Controllers;

[Authorize(Roles = "Center,Teacher")]
public class TeachersController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITeachersCrud _teachersCrud;

    public TeachersController(IUnitOfWork unitOfWork, ITeachersCrud teachersCrud)
    {
        _unitOfWork = unitOfWork;
        _teachersCrud = teachersCrud;
    }

    [Authorize(Roles = "Center")]
    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> GetAll()
    {
        if (!int.TryParse(Request.Cookies["centerId"], out var centerId))
            return Content("No Logged In Center");
        var teachers = await _unitOfWork.Teachers.GetAll(centerId);
        return Json(teachers);
    }

    public IActionResult Details(int id)
    {
        var teacher = _unitOfWork.Teachers.GetById(id).Result;
        if (teacher is null)
            return Content("Teacher Not Found");
        var teacherVm = new GetTeacherByIdVm()
        {
            Id = teacher.Id,
            Name = teacher.Name,
            Salary = teacher.Salary,
            Age = teacher.Age,
            CourseId = teacher.CourseId ?? 0,
            CourseName = teacher.Course?.Name ?? string.Empty,
            Image = teacher.Image
        };
        var option = new CookieOptions();
        Response.Cookies.Append("teacherId", teacher.Id.ToString(), option);
        return View(teacherVm);
    }

    public async Task<IActionResult> TeacherCourseStudents(int id)
    {
        var course = await _unitOfWork.Courses.GetById(id);
        if (course is null)
            return Content("Course Not Found");
        var students = _unitOfWork.StudentCourses
            .GetAllFiltered(x => x.CourseId == course.Id).Result
            .Select(x => new StudentCourseVm()
            {
                Id = x.Id,
                StudentId = x.StudentId,
                StudentName = x.Student.Name,
                CourseId = x.CourseId,
                CourseName = x.Course.Name,
                Grade = x.Grade
            })
            .ToList();

        return View(students);
    }

    /*public IActionResult GetAllTeacherCourseStudents(int courseId)
    {
        var students = _unitOfWork.StudentCourses
            .GetAllFiltered(x => x.CourseId == courseId).Result
            .Select(x => new StudentCourseVm()
            {
                Id = x.Id,
                StudentId = x.StudentId,
                StudentName = x.Student.Name,
                CourseId = x.CourseId,
                CourseName = x.Course.Name,
                Grade = x.Grade
            })
            .ToList();
        return Json(students);
    }*/

    [HttpGet]
    [Authorize(Roles = "Center")]
    public IActionResult Create(CreateTeacherVm teacherVm)
    {
        if (!int.TryParse(Request.Cookies["centerId"], out var centerId))
            return Content("No Logged In Center");
        teacherVm.Courses = _unitOfWork.Courses.GetCenterCoursesSelectList(centerId);
        return View(teacherVm);
    }

    [Authorize(Roles = "Center")]
    [HttpPost]
    public async Task<IActionResult> SaveCreate([FromForm] CreateTeacherVm teacherVm)
    {
        if (!int.TryParse(Request.Cookies["centerId"], out var centerId))
            return Json(new { IsSuccess = false, Message = "No Logged In Center" });
        teacherVm.CenterId = centerId;
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            var message = string.Join(", ", errors);
            return Json(new { IsSuccess = false, Message = message });
        }

        var result = await _teachersCrud.Create(teacherVm);
        return Json(new { result.IsSuccess, result.Message });
    }

    [HttpGet]
    [Authorize(Roles = "Center")]
    public IActionResult Update(int id)
    {
        var teacher = _unitOfWork.Teachers.GetById(id).Result;
        if (teacher is null)
            return Content("Teacher Not Found");
        TempData["teacherId"] = id;
        var teacherVm = new UpdateTeacherVm()
        {
            Name = teacher.Name,
            Salary = teacher.Salary,
            Age = teacher.Age,
            Image = teacher.Image,
            CourseId = teacher.CourseId,
            CenterId = teacher.CenterId,
            Courses = _unitOfWork.Courses.GetCenterCoursesSelectList(teacher.CenterId)
        };
        return View(teacherVm);
    }

    [HttpPost]
    [Authorize(Roles = "Center")]
    public async Task<IActionResult> Update([FromForm] UpdateTeacherVm teacherVm)
    {
        if (TempData["teacherId"] is not int teacherId)
            return Json(new { IsSuccess = false, Message = "Invalid Course Id" });
        teacherVm.Id = teacherId;
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(x => x.ErrorMessage);
            var message = string.Join(',', errors);
            return Json(new { IsSuccess = false, Message = message });
        }

        var result = await _teachersCrud.Update(teacherVm);
        return Json(new { result.IsSuccess, result.Message });
    }

    [Authorize(Roles = "Center")]
    public async Task<IActionResult> Delete(int id)
    {
        var teacher = await _unitOfWork.Teachers.GetById(id);
        if (teacher is null)
            return Content("Teacher Not Found");
        return View(id);
    }

    [Authorize(Roles = "Center")]
    public async Task<IActionResult> ConfirmDeletion(int id)
    {
        var result = await _teachersCrud.Delete(id);
        return result.IsSuccess
            ? RedirectToAction("Index", "Teachers")
            : Content(result.Message);
    }


    [Authorize(Roles = "Teacher")]
    [HttpPost]
    public async Task<IActionResult> SetStudentsGrade(List<StudentCourseVm> courseStudentVms)
    {
        var courseStudents = await _unitOfWork.StudentCourses
            .GetAllFiltered(cs => courseStudentVms
                .Select(x => x.Id).Contains(cs.Id));
        foreach (var student in courseStudents)
        {
            var studentCourseVm = courseStudentVms.FirstOrDefault(x => x.Id == student.Id);
            if (studentCourseVm is not null)
                student.Grade = studentCourseVm.Grade;
        }

        await _unitOfWork.StudentCourses.UpdateRange(courseStudents);
        await _unitOfWork.Complete();
        var teacherId = Request.Cookies["teacherId"];
        return RedirectToAction("Details", "Teachers", new { id = teacherId });
    }
}