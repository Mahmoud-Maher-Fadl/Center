using Core.common;
using Core.ViewModels.Teachers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Core.Models.Teacher;

public interface ITeacherRepository:IBaseRepository<Teacher>
{
    Task<List<GetAllTeachersVm>> GetAll(int? centerId=0);
   // Task<GetTeacherByIdVm?> GetById(int id);
   
    Task<Teacher?> GetById(int? id);
    IEnumerable<SelectListItem> GetCenterTeachersSelectList(int centerId);
    Task<Teacher?> GetByUserId(string userId);

    
}