using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleUrlList.Interfaces;
using SimpleUrlList.Models;

namespace SimpleUrlList.Web.Pages.Groups;

public class LinksPageModel(ILogger<LinksPageModel> logger, ILinkGroupRepository linkGroupRepository) : PageModel
{
    public async Task OnGetAsync()
    {
        logger.LogInformation("Called Links endpoint at {DateCalled} from id {LinkGroupId}", DateTime.UtcNow,
            LinkGroupId);
        CurrentLinkGroup = await linkGroupRepository.DetailsAsync(LinkGroupId);
        logger.LogInformation("Found {LinkCount} links for group {LinkGroupId}", CurrentLinkGroup.Links.Count,
            LinkGroupId);
    }

    [BindProperty(SupportsGet = true)] public string LinkGroupId { get; set; }
    [BindProperty] public LinkGroup CurrentLinkGroup { get; set; }
}