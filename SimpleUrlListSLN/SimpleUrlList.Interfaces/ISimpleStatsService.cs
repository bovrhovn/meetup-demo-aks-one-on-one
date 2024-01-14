using SimpleUrlList.Models;

namespace SimpleUrlList.Interfaces;

public interface ISimpleStatsService
{
    Task<StatInfo> GetForLinkGroupAsync(string linkGroupId);
    Task<bool> SaveAsync(StatInfo statInfo);
}