namespace SimpleUrlList.Models;

public class Category
{
    public string CategordId { get; set; }
    public string Name { get; set; }
    public List<LinkGroup> Groups { get; set; } = new();
}