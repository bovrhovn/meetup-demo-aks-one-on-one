using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleUrlList.Interfaces;
using SimpleUrlList.Models;
using SimpleUrlList.Web.Base;

namespace SimpleUrlList.Web.Pages.Groups;

[Authorize]
public class CreatePageModel(
    ILogger<CreatePageModel> logger,
    ILinkGroupRepository linkGroupRepository,
    IUserDataContext userDataContext,
    ICategoryRepository categoryRepository) : BasePageModel
{
    public async Task OnGetAsync()
    {
        logger.LogInformation("Loading create page for link group {DateCreated}", DateTime.Now);
        if (!string.IsNullOrEmpty(LinkGroupId))
        {
            logger.LogInformation("Getting link group {LinkGroupId}", LinkGroupId);
            CreateLinkGroup = await linkGroupRepository.DetailsAsync(LinkGroupId);
        }

        Categories = await categoryRepository.GetAsync();
        logger.LogInformation("Loading categories {CategoriesCount}", Categories.Count);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        //getting categories
        var form = await Request.ReadFormAsync();
        var categoryId = form["ddlCategory"];

        CreateLinkGroup.Category = new Category { CategoryId = categoryId };
        var userViewModel = userDataContext.GetCurrentUser();
        CreateLinkGroup.User = new SulUser { UserId = userViewModel.UserId };
        logger.LogInformation("Setting category {CategoryId} and user {UserId}", categoryId, userViewModel.UserId);
        try
        {
            if (string.IsNullOrEmpty(LinkGroupId))
            {
                logger.LogInformation("Inserting link group {LinkGroupName}", CreateLinkGroup.Name);
                await linkGroupRepository.InsertAsync(CreateLinkGroup);
            }
            else
            {
                logger.LogInformation("Updating link group {LinkGroupName} for {LinkGroupId}", CreateLinkGroup.Name,
                    LinkGroupId);
                CreateLinkGroup.Links = new();
                await linkGroupRepository.UpdateAsync(CreateLinkGroup);
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error saving link group {LinkGroupName}", CreateLinkGroup.Name);
            Message = "Error occured";
            return Page();
        }

        return RedirectToPage("/Groups/Links", new { linkGroupId = LinkGroupId });
    }

    [BindProperty(SupportsGet = true)] public string LinkGroupId { get; set; }
    [BindProperty] public LinkGroup CreateLinkGroup { get; set; }
    [BindProperty] public List<Category> Categories { get; set; }
}