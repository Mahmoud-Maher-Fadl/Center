using Core.common;
using Core.Models.Center;
using Infrastructure.common;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Center;

public class CenterRepository : BaseRepository<Core.Models.Center.Center>, ICenterRepository
{
    private readonly ApplicationDbContext _context;

    public CenterRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public Task<List<Core.Models.Center.Center>> GetAll(int? id)
    {
        
        return _context.Centers.ToListAsync();
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