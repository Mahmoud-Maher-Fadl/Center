using Core.common;
using Core.ViewModels.Center;
using Microsoft.AspNetCore.Authorization;
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
        return View();
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult GetCenters()
    {
        var centers = _unitOfWork.Centers.GetAll().Result;
        var recordsTotal = centers.Count;
        var jsonData = new { recordsFiltered = recordsTotal, recordsTotal, data = centers };
        return Json(jsonData);
        //return Json(centers);
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