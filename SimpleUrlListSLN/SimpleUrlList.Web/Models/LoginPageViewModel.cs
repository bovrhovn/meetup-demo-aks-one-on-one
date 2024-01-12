using System.ComponentModel.DataAnnotations;

namespace SimpleUrlList.Web.Models;

public class LoginPageViewModel
{
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
}