using Core.common;
using Core.Models.Student;
using Core.Models.StudentCourse;
using Core.Models.User;
using Core.ViewModels.Students;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Presentation.Validations.Interfaces;

namespace Presentation.Validations.Implementations;

public class StudentsCrud : IStudentsCrud
{
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public StudentsCrud(UserManager<User> userManager, IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<(bool IsSuccess, string Message)> Create(CreateStudentVm studentVm)
    {
        await using var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            var user = await _userManager.FindByEmailAsync(studentVm.Email);
            if (user != null)
                return (IsSuccess: false, Message: "User Already Exists");

            user = new User
            {
                Email = studentVm.Email,
                UserName = studentVm.Email
            };

            var createUserResult = await _userManager.CreateAsync(user, studentVm.Password);
            if (!createUserResult.Succeeded)
                return (false, createUserResult.Errors.ToString() ?? "");

            var assignRoleResult = await _userManager.AddToRoleAsync(user, "Student");
            if (!assignRoleResult.Succeeded)
                return (false, assignRoleResult.Errors.ToString() ?? "");

            var student = new Student()
            {
                Name = studentVm.Name,
                Address = studentVm.Address,
                SSN = studentVm.SSN,
                Age = studentVm.Age,
                CenterId = studentVm.CenterId,
                UserId = user.Id
            };

            var courses = studentVm.SelectedCourses
                .Select(x => new StudentCourse()
                {
                    CourseId = x
                })
                .ToHashSet();

            student.StudentCourses = courses;

            await _unitOfWork.Students.Add(student);
            await _unitOfWork.Complete();
            await transaction.CommitAsync();
            return (true, "Student Created Successfully");

        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return (false, $"An error occurred: {ex.InnerException?.Message}");
        }
    }

    public async Task<(bool IsSuccess, string Message)> Update(UpdateStudentVm studentVm)
    {
        var student = _unitOfWork.Students.GetById(studentVm.Id).Result;
        if (student is null)
            return (false, "Student Not Found");
        /*
        if (studentVm.SelectedCourses is not null
            && studentVm.SelectedCourses.Count > 0
            && studentVm.CenterId != student.CenterId)
            return (false, "Courses isn't in this Center");

        */
        student.Name = studentVm.Name;
        student.Address = studentVm.Address;
        student.SSN = studentVm.Ssn;
        student.Age = studentVm.Age;

        if (studentVm.SelectedCourses is not null && studentVm.SelectedCourses.Count > 0)
            student.StudentCourses = studentVm.SelectedCourses
                .Where(x => x != 0)
                .Select(x => new StudentCourse()
                {
                    StudentId = student.Id,
                    CourseId = x,
                }).ToHashSet();
        await _unitOfWork.Students.Update(student);
        await _unitOfWork.Complete();
        return (true, "Student Updated Successfully");
    }


    public async Task<(bool IsSuccess, string Message)> Delete(int id)
    {
        var student = _unitOfWork.Students.GetById(id).Result;
        if (student is null)
            return (false, "Student Not Found");
        var studentCourses = _unitOfWork.StudentCourses
            .GetAllFiltered(x => x.StudentId == id)
            .Result;
        await _unitOfWork.StudentCourses.DeleteRange(studentCourses);
        await _unitOfWork.Students.DeleteById(id);
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == student.UserId);
        if (user is not null)
        {
            var deleteUserResult = await _userManager.DeleteAsync(user);
            if (!deleteUserResult.Succeeded)
                return (false, deleteUserResult.Errors.ToString() ?? "");
        }

        await _unitOfWork.Complete();
        return (true, "Student Deleted Successfully");
    }

    /*public async Task<(bool IsSuccess, string Message)> StudentCenterCourses(int studentId)
    {
        var student = await _unitOfWork.Students.GetById(studentId);
        if (student is null)
            return (false, "Student Not Found");
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
                //Selected = studentCourses.Any(sc => sc.CourseId == x.Id)
            }).ToList(),
            SelectedCourses = studentCourses.Select(x => x.CourseId).ToList()
        };

    }*/
}