using Core.common;
using Core.Models.Center;
using Core.Models.User;
using Core.ViewModels.Center;
using Microsoft.AspNetCore.Identity;
using Presentation.Validations.Interfaces;
using System.Linq;
namespace Presentation.Validations.Implementations;

public class CentersCrud : ICentersCrud
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;

    public CentersCrud(IUnitOfWork unitOfWork, UserManager<User> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task<(bool IsSuccess, string Message)> Create(CreateCenterVm center)
    {
        var user = _userManager.Users.FirstOrDefault(x => x.Email == center.Email);
        if (user is not null)
            return (false, "User Already Exists");
        user = new User
        {
            Email = center.Email,
            UserName = center.Email
        };
        var createUserResult = await _userManager.CreateAsync(user, center.Password);
        if (!createUserResult.Succeeded)
            return (IsSuccess: false, Message: $"Error Creating User: {createUserResult.Errors}");
        var assignRoleResult = await _userManager.AddToRoleAsync(user, "Center");
        if (!assignRoleResult.Succeeded)
            return (false, $"Error Assigning Role: {assignRoleResult.Errors}");
        var entity = new Center()
        {
            Name = center.Name,
            Location = center.Location,
            UserId = user.Id
        };
        await _unitOfWork.Centers.Add(entity);
        await _unitOfWork.Complete();
        return (true, "Center Created Successfully");
    }

    public async Task<(bool IsSuccess, string Message)> Update(int id, UpdateCenterVm center)
    {
        var entity = await _unitOfWork.Centers.GetById(id);
        if (entity is null)
            return (false, "Center Not Found");
        entity.Name = center.Name;
        entity.Location = center.Location;
        await _unitOfWork.Centers.Update(entity);
        await _unitOfWork.Complete();
        return (true, "Center Updated Successfully");
    }

    public async Task<(bool IsSuccess, string Message)> Delete(int id)
    {
        var center = await _unitOfWork.Centers.GetById(id);
        if (center is null)
            return (true, "Center Not Found");
        center.IsDeleted = true;
        await _unitOfWork.Centers.Update(center);
        await _unitOfWork.Complete();
        return (true, "Center Deleted Successfully");
    }
}