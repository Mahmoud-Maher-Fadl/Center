using Core.common;
using Core.ViewModels.Students;

namespace Core.Models.Student;

public interface IStudentRepository:IBaseRepository<Student>
{
    Task<List<GetAllStudentsVm>> GetAll(int? centerId=0);
    Task<Student?> GetById(int id);
    Task<Student?> GetByUserId(string userId);

}