using Htmx;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleUrlList.Interfaces;
using SimpleUrlList.Models;

namespace SimpleUrlList.Web.Pages.Groups;

public class IndexPageModel(ILogger<IndexPageModel> logger, ILinkGroupRepository linkGroupRepository) : PageModel
{
    public async Task<IActionResult> OnGetAsync(int? pageNumber)
    {
        logger.LogInformation("Loading index page for orders {DateCreated}", DateTime.Now);
        var page = pageNumber ?? 1;

        logger.LogInformation("Getting groups for page {Page} with query {Query}", page, Query);
        var groups = await linkGroupRepository.SearchAsync(page, 20, Query);
        Groups = groups;
        logger.LogInformation("Received {Count} groups for page {Page} with query {Query}", groups.Count, page, Query);
        
        if (!Request.IsHtmx()) return Page();

        return Partial("_LinkGroups", groups);
    }

    [BindProperty(SupportsGet = true)] public string Query { get; set; }
    [BindProperty] public List<LinkGroup> Groups { get; set; }
}