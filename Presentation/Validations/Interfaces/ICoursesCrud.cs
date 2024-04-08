using Core.ViewModels.Courses;

namespace Presentation.Validations.Interfaces;

public interface ICoursesCrud
{
    Task<(bool IsSuccess, string Message)> Create(CreateCourseVm courseVm);
    Task<(bool IsSuccess, string Message)> Update(UpdateCourseVm courseVm);
    Task<(bool IsSuccess, string Message)> Delete(int id);
}