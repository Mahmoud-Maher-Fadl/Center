using Core.ViewModels.Teachers;

namespace Presentation.Validations.Interfaces;

public interface ITeachersCrud
{
    Task<(bool IsSuccess, string Message)> Create(CreateTeacherVm teacherVm);
    Task<(bool IsSuccess, string Message)> Update(UpdateTeacherVm teacherVm);
    Task<(bool IsSuccess, string Message)> Delete(int id);
}