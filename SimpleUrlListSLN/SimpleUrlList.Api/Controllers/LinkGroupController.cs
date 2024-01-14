using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleUrlList.Shared;

namespace SimpleUrlList.Api.Controllers;

[ApiController,
 Route(ConstantRouteHelper.LinkGroupBaseRoute),
 Produces(MediaTypeNames.Application.Json)]
public class LinkGroupController(ILogger<LinkGroupController> logger) : Controller
{
    [HttpGet]
    [Route(ConstantRouteHelper.HealthRoute)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [AllowAnonymous]
    public IActionResult IsAlive()
    {
        logger.LogInformation("Called alive endpoint at {DateCalled}", DateTime.UtcNow);
        return new ContentResult { StatusCode = 200, Content = $"I am alive at {DateTime.Now}" };
    }
}