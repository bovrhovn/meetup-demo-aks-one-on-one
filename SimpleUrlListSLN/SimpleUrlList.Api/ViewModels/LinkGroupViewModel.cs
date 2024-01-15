using SimpleUrlList.Models;

namespace SimpleUrlList.Api.ViewModels;

public class LinkGroupViewModel
{
    public string LinkGroupId { get; set; }
    public List<Link> Links { get; set; }
}