using Microsoft.AspNetCore.Mvc;
using SimpleUrlList.Web.Base;

namespace SimpleUrlList.Web.Pages.User;

public class LogoutPageModel(ILogger<LogoutPageModel> logger, IUserDataContext userDataContext)
    : BasePageModel
{
    public async Task<RedirectToPageResult> OnGetAsync()
    {
        var fullName = userDataContext.GetCurrentUser().Fullname;
        logger.LogInformation("Logged out page loaded at {DateLoaded}.", DateTime.Now);
        
        await userDataContext.LogOut();
        
        Message = $"User {fullName} has been logged out";
        logger.LogInformation("User {CurrentUser} logged out at {DateLoaded}.", fullName, DateTime.Now);
        return RedirectToPage("/Info/Index");
    }
}