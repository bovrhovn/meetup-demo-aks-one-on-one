using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleUrlList.Interfaces;
using SimpleUrlList.Models;
using SimpleUrlList.Web.Base;

namespace SimpleUrlList.Web.Pages.User;

[Authorize]
public class DashboardPageModel(
    ILogger<DashboardPageModel> logger,
    IUserDataContext userDataContext,
    ILinkGroupRepository linkGroupRepository) : BasePageModel
{
    public async Task OnGetAsync()
    {
        var userViewModel = userDataContext.GetCurrentUser();
        logger.LogInformation("Loading dashboard for user {User} - starting at {DateStart}", userViewModel.Fullname,
            DateTime.Now);
        MyLinkGroups = await linkGroupRepository.GetFromSpecificUserAsync(userViewModel.UserId);
        logger.LogInformation("Loading dashboard for user {User} - finished at {DateEnd} - with {LinkGroupCount}",
            userViewModel.Fullname,
            DateTime.Now, MyLinkGroups.Count);
    }

    [BindProperty] public List<LinkGroup> MyLinkGroups { get; set; }
}