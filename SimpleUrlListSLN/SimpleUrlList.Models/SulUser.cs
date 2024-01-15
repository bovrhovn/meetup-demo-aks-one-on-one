namespace SimpleUrlList.Models;

public class SulUser
{
    public Guid UserId { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public List<LinkGroup> Groups { get; set; } = new();
}