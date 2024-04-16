using Core.common;
using Core.ViewModels.Center;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Validations.Interfaces;

namespace Presentation.Controllers;

[Authorize(Roles = "Admin,Center")]
public class CenterController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICentersCrud _centersCrud;

    public CenterController(IUnitOfWork unitOfWork, ICentersCrud centersCrud)
    {
        _unitOfWork = unitOfWork;
        _centersCrud = centersCrud;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult Index()
    {
        var requestCulture = Request.HttpContext.Features.Get<IRequestCultureFeature>()?.RequestCulture.Culture.Name;
        Console.WriteLine(requestCulture);
        return View();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> GetAll()
    {
        var pageSize = int.Parse(Request.Form["length"]);
        var skip = int.Parse(Request.Form["start"]);
        var search = Request.Form["search[value]"];
        var orderColumn = Request.Form[string.Concat("columns[", Request.Form["order[0][column]"], "][name]")];
        var orderColumnDirection = Request.Form["order[0][dir]"];
        var (data, recordsTotal) = await _unitOfWork.Centers.GetAll(search: search, orderBy: orderColumn, orderDirection: orderColumnDirection, skip: skip, take: pageSize);
        var jsonData = new { recordsFiltered = recordsTotal, recordsTotal, data };
        return Json(jsonData);
    }


    [Authorize(Roles = "Center")]
    public IActionResult Details(int id)
    {
        var center = _unitOfWork.Centers.GetById(id).Result;
        if (center is null)
            return Content("Center Not Found");
        var option = new CookieOptions();
        Response.Cookies.Append("centerId", center.Id.ToString(), option);
        return View(center);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Create(CreateCenterVm center)
    {
        return View(center);
    }
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> SaveCreate(CreateCenterVm center)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            var message = string.Join(", ", errors);
            return Json(new { IsSuccess = false, Message = message });
        }

        var result = await _centersCrud.Create(center);
        return Json(new { result.IsSuccess, result.Message });
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult Update(int id)
    {
        var center = _unitOfWork.Centers.GetById(id).Result;
        if (center is null)
            return Content("Center Not Found");
        var centerVm = new UpdateCenterVm
        {
            Id = center.Id,
            Name = center.Name,
            Location = center.Location
        };
        return View(centerVm);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, UpdateCenterVm center)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(x => x.ErrorMessage)
                .ToList();
            var message = string.Join(',', errors);
            return Json(new { IsSuccess = false, Message = message });
        }

        var result = await _centersCrud.Update(id, center);
        return Json(new { result.IsSuccess, result.Message });
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult Delete(int id)
    {
        return View(id);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult ConfirmDeletion(int id)
    {
        return _centersCrud.Delete(id).Result.IsSuccess
            ? RedirectToAction(nameof(Index))
            : Content("Center Not Found");
    }
}