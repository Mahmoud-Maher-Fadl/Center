using Core.common;
using Core.Models.Teacher;
using Core.Models.User;
using Core.ViewModels.Teachers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.Validations.Interfaces;

namespace Presentation.Validations.Implementations;

public class TeachersCrud : ITeachersCrud
{
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly string _imagesPath;


    public TeachersCrud(UserManager<User> userManager, IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _imagesPath = $"{webHostEnvironment.WebRootPath}/assets/images/teachers/";
    }

    public async Task<(bool IsSuccess, string Message)> Create(CreateTeacherVm teacherVm)
    {
        var user = _userManager.Users.FirstOrDefault(x => x.Email == teacherVm.Email);
        if (user is not null)
            return (IsSuccess: false, Message: "User Already Exists");
        user = new User
        {
            Email = teacherVm.Email,
            UserName = teacherVm.Email
        };
        var createUserResult = await _userManager.CreateAsync(user, teacherVm.Password);
        if (!createUserResult.Succeeded)
        {
            var errors = createUserResult.Errors
                .Select(v => v.Description)
                .ToList();
            var message = string.Join(", ", errors);
            return (IsSuccess: false,
                Message: $"Error Creating User: {message}");
        }

        var assignRoleResult = await _userManager.AddToRoleAsync(user, "Teacher");
        if (!assignRoleResult.Succeeded)
        {
            var errors = createUserResult.Errors
                .Select(x => x.Description)
                .ToString();
            return (IsSuccess: false, Message: $"Error Assigning Role: {errors}");
        }

        var teacher = new Teacher()
        {
            Name = teacherVm.Name,
            Salary = teacherVm.Salary,
            Age = teacherVm.Age,
            CenterId = teacherVm.CenterId,
            CourseId = teacherVm.CourseId,
            UserId = user.Id
        };
        if (teacherVm.Image is not null)
        {
            var image = await SaveCover(teacherVm.Image);
            teacher.Image = image;
        }

        await _unitOfWork.Teachers.Add(teacher);
        var effected = await _unitOfWork.Complete();
        if (effected > 0)
            return (true, "Teacher Created Successfully");
        // return RedirectToAction("Index", "Teachers"); //, new { centerId = teacherVm.CenterId });
        if (teacher.Image is not null && File.Exists(Path.Combine(_imagesPath, teacher.Image)))
            File.Delete(Path.Combine(_imagesPath, teacher.Image));
        await _userManager.DeleteAsync(user);
        return (false, "Error Creating Teacher");
    }

    public async Task<(bool IsSuccess, string Message)> Update(UpdateTeacherVm teacherVm)
    {
        var teacher = _unitOfWork.Teachers.GetById(teacherVm.Id).Result;
        if (teacher is null)
            return (false, "Teacher Not Found");
        if (teacherVm.CourseId.HasValue)
        {
            var course = _unitOfWork.Courses.GetById(teacherVm.CourseId.Value).Result;
            if (course is null || course.CenterId != teacher.CenterId)
                return (false, "Course Doesn't exist in this Center");
        }

        teacher.Name = teacherVm.Name;
        teacher.Salary = teacherVm.Salary;
        teacher.Age = teacherVm.Age;
        teacher.CourseId = teacherVm.CourseId;
        var hasNewCover = teacherVm.Cover is not null;
        if (hasNewCover)
        {
            teacherVm.CurrentCover = teacher.Image ?? "";
            var coverName = await SaveCover(teacherVm.Cover!);
            teacher.Image = coverName;
        }

        await _unitOfWork.Teachers.Update(teacher);
        var affected = await _unitOfWork.Complete();
        if (affected > 0)
        {
            if (hasNewCover)
            {
                var oldCoverPath = Path.Combine(_imagesPath, teacherVm.CurrentCover ?? "");
                if (File.Exists(oldCoverPath))
                    File.Delete(oldCoverPath);
            }

            return (true, "Teacher Updated Successfully");
        }

        return (false, "Error Updating Teacher");
    }

    public async Task<(bool IsSuccess, string Message)> Delete(int id)
    {
        var teacher = await _unitOfWork.Teachers.GetById(id);
        if (teacher is null)
            return (false, "Teacher Not Found");
        await _unitOfWork.Teachers.DeleteById(id);
        var effected = await _unitOfWork.Complete();
        if (effected <= 0)
            return (false, "Error Deleting Teacher");
        // delete the associated user
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == teacher.UserId);
        if (user is not null)
            await _userManager.DeleteAsync(user);

        if (teacher.Image is not null)
        {
            var path = Path.Combine(_imagesPath, teacher.Image);
            if (File.Exists(path))
                File.Delete(path);
        }

        return (true, "Teacher Deleted Successfully");
    }


    private async Task<string> SaveCover(IFormFile cover)
    {
        var coverName = $"{Guid.NewGuid()}{Path.GetExtension(cover.FileName)}";
        var path = Path.Combine(_imagesPath, coverName);
        await using var stream = File.Create(path);
        await cover.CopyToAsync(stream);
        return coverName;
    }
}