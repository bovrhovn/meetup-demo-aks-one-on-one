using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleUrlList.Interfaces;
using SimpleUrlList.Models;

namespace SimpleUrlList.Web.Pages.Groups;

public class CreatePageModel(ILogger<CreatePageModel> logger, ILinkGroupRepository linkGroupRepository) : PageModel
{
    public void OnGet()
    {
        logger.LogInformation("Loading create page for orders {DateCreated}", DateTime.Now);
    }

    [BindProperty]
    public LinkGroup CreateLinkGroup { get; set; }
}