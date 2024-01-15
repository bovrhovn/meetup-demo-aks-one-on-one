namespace SimpleUrlList.Models;

public class Category
{
    public Guid CategoryId { get; set; }
    public string Name { get; set; }
    public List<LinkGroup> Groups { get; set; } = new();
}