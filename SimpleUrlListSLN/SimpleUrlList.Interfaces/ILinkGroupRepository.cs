using SimpleUrlList.Models;

namespace SimpleUrlList.Interfaces;

public interface ILinkGroupRepository : IDataRepository<LinkGroup>
{
    Task<List<LinkGroup>> GetFromSpecificUserAsync(string userId);
}