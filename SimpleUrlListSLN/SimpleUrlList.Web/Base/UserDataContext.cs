using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using SimpleUrlList.Web.Models;

namespace SimpleUrlList.Web.Base;

public class UserDataContext(IHttpContextAccessor httpContextAccessor) : IUserDataContext
{
    public UserViewModel GetCurrentUser()
    {
        var httpContextUser = httpContextAccessor.HttpContext?.User;

        var currentUser = new UserViewModel();
        var claimName = httpContextUser?.FindFirst(ClaimTypes.Name);
        currentUser.Fullname = claimName!.Value;

        var claimId = httpContextUser?.FindFirst(ClaimTypes.NameIdentifier);
        currentUser.UserId = Guid.Parse(claimId!.Value);

        var claimEmail = httpContextUser!.FindFirst(ClaimTypes.Email);
        currentUser.Email = claimEmail!.Value;

        return currentUser;
    }

    public Task LogOut() => httpContextAccessor.HttpContext!.SignOutAsync();
}

