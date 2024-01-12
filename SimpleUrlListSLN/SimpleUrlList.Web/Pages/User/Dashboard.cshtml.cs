using Microsoft.AspNetCore.Authorization;
using SimpleUrlList.Web.Base;

namespace SimpleUrlList.Web.Pages.User;

[Authorize]
public class DashboardPageModel(
    ILogger<DashboardPageModel> logger,
    IUserDataContext userDataContext) : BasePageModel
{
    public void OnGet()
    {
        var userViewModel = userDataContext.GetCurrentUser();
        logger.LogInformation("Loading dashboard for user {User} - starting at {DateStart}", userViewModel.Fullname,
            DateTime.Now);
    }
}