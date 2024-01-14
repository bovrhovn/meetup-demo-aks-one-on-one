using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleUrlList.Interfaces;
using SimpleUrlList.Web.Base;
using SimpleUrlList.Web.Models;

namespace SimpleUrlList.Web.Pages.User;

[AllowAnonymous]
public class LoginPageModel(ILogger<LoginPageModel> logger, IUserService userService)
    : BasePageModel
{
    public void OnGet() => logger.LogInformation("Load page at {DateLoaded}", DateTime.Now);

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrEmpty(LoginData.Email) || string.IsNullOrEmpty(LoginData.Password))
        {
            Message = "Username or password is mandatory";
            logger.LogWarning("Username or password is mandatory - data was empty at {DateLoaded}", DateTime.Now);
            return Page();
        }

        var user = await userService.LoginAsync(LoginData.Email, LoginData.Password);

        if (user == null)
        {
            Message = "Check username and password, login was unsuccessful";
            return Page();
        }

        await HttpContext.SignInAsync(user.GenerateClaims());

        return RedirectToPage("/User/Dashboard");
    }


    [BindProperty] public LoginPageViewModel LoginData { get; set; }
}