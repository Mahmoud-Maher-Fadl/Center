using System.ComponentModel.DataAnnotations;

namespace Core.ViewModels.Center;

public class CreateCenterVm : BaseCenterVm
{
    [Required(ErrorMessage = "Email is Required"), EmailAddress(ErrorMessage = "Invalid Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is Required")]
    public string Password { get; set; } = string.Empty;

    [Required, Compare(nameof(Password)), Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; } = string.Empty;
}