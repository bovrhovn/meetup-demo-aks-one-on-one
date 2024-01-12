using SimpleUrlList.Web.Models;

namespace SimpleUrlList.Web.Base;

public interface IUserDataContext
{
    UserViewModel GetCurrentUser();
    Task LogOut();
}