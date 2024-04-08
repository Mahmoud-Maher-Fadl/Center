using Core.ViewModels.Students;

namespace Presentation.Validations.Interfaces;

public interface IStudentsCrud
{
    Task<(bool IsSuccess, string Message)> Create(CreateStudentVm studentVm);
    Task<(bool IsSuccess, string Message)> Update(UpdateStudentVm studentVm);
    Task<(bool IsSuccess, string Message)> Delete(int id);
}