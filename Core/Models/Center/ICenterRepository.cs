using Core.common;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Core.Models.Center;

public interface ICenterRepository:IBaseRepository<Center>
{
    Task<List<Center>>  GetAll(int? id=0);
    Task<Center?> GetById(int id);
    Task<Center?> GetByUserId(string userId);
    IEnumerable<SelectListItem> GetCentersSelectList();
}