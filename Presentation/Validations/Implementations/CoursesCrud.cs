using Core.common;
using Core.Models.Course;
using Core.ViewModels.Courses;
using Presentation.Validations.Interfaces;

namespace Presentation.Validations.Implementations;

public class CoursesCrud: ICoursesCrud
{
    private readonly IUnitOfWork _unitOfWork;

    public CoursesCrud(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<(bool IsSuccess, string Message)> Create(CreateCourseVm courseVm)
    {
        var course = new Course
        {
            Name = courseVm.Name,
            Hours = courseVm.Hours,
            Price = courseVm.Price,
            CenterId = courseVm.CenterId
        };
        await _unitOfWork.Courses.Add(course);
        await _unitOfWork.Complete();
        return (IsSuccess: true, Message: "True");
    }

    public async Task<(bool IsSuccess, string Message)> Update(UpdateCourseVm courseVm)
    {
        var course = _unitOfWork.Courses.GetById(courseVm.Id).Result;
        if (course is null)
            return (false, "Course Not Found");
        course.Name = courseVm.Name;
        course.Hours = courseVm.Hours;
        course.Price = courseVm.Price;
        await _unitOfWork.Courses.Update(course);
        await _unitOfWork.Complete();
        return (true, "Course Updated Successfully");
    }

    public async Task<(bool IsSuccess, string Message)> Delete(int id)
    {
        var course = _unitOfWork.Courses.GetById(id).Result;
        if (course is null)
            return (false, "Course Not Found");
        if (course.StudentCourse.Count > 0)
            return (false, "Course has Students register in");
        await _unitOfWork.Courses.DeleteById(id);
        await _unitOfWork.Complete();
        return (true, "Course Deleted Successfully");
    }
}