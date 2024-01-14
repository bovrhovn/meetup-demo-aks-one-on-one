namespace SimpleUrlList.Models;

public class Link
{
    public string LinkId { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public LinkGroup Group { get; set; }
}