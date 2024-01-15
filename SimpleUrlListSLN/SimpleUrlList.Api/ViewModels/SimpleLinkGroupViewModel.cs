using SimpleUrlList.Models;

namespace SimpleUrlList.Api.ViewModels;

public class SimpleLinkGroupViewModel
{
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<Link> Links { get; set; } = new();
}