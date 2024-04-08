using Core.Models.Center;
using Core.ViewModels.Center;

namespace Presentation.Validations.Interfaces;

public interface ICentersCrud
{
    Task<(bool IsSuccess, string Message)> Create(CreateCenterVm center);
    Task<(bool IsSuccess, string Message)> Update(int id, UpdateCenterVm center);
    Task<(bool IsSuccess, string Message)> Delete(int id);
}