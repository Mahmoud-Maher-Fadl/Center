using System.Linq;
using Core.common;
using Core.ViewModels.Center;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Core.Models.Center;

public interface ICenterRepository : IBaseRepository<Center>
{
    Task<(List<GetAllCentersVm> data, int countData)> GetAll(string? search = "", string? orderBy = "",
        string? orderDirection = "", int skip = 0, int take = 0);

    Task<Center?> GetById(int id);
    Task<Center?> GetByUserId(string userId);
    IEnumerable<SelectListItem> GetCentersSelectList();
}