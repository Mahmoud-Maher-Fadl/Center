using System.Linq.Dynamic.Core;
using Core.Models.Center;
using Core.ViewModels.Center;
using Infrastructure.common;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Center;

public class CenterRepository : BaseRepository<Core.Models.Center.Center>, ICenterRepository
{
    private readonly ApplicationDbContext _context;

    public CenterRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<(List<GetAllCentersVm> data, int countData)> GetAll(string? search = "", string? orderBy = "",
        string? orderDirection = "", int skip = 0, int take = 0)
    {
        var centers = _context.Centers
            .Where(x =>
                string.IsNullOrEmpty(search)
                || string.IsNullOrWhiteSpace(search)
                || x.Name.Contains(search)
                || x.Location.Contains(search))
            .Select(x => new GetAllCentersVm()
            {
                Id = x.Id,
                Name = x.Name,
                Location = x.Location
            });
        var count = centers.Count();
        centers = centers.Skip(skip).Take(take); /* if it in one step with where cond
                                                  the pagination won't work cause of the count must b for all data*/
        if (orderBy.IsNullOrEmpty())
            return (await centers.ToListAsync(), count);

        centers = centers.AsQueryable()
            .OrderBy(orderBy + " " + orderDirection);
        return (await centers.ToListAsync(), count);
    }

    public Task<Core.Models.Center.Center?> GetById(int id)
    {
        return _context.Centers.Where(x => x.Id == id)
            .Include(x => x.Teachers)
            .Include(x => x.Courses)
            .Include(x => x.Students)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Core.Models.Center.Center?> GetByUserId(string userId)
    {
        var center = await _context.Centers
            .FirstOrDefaultAsync(x => x.UserId == userId);
        return center;
    }

    public IEnumerable<SelectListItem> GetCentersSelectList()
    {
        return _context.Centers
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Name
            });
    }
}