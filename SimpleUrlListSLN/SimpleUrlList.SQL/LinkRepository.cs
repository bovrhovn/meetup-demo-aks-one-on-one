using System.Data.SqlClient;
using Dapper;
using SimpleUrlList.Interfaces;
using SimpleUrlList.Models;
using SimpleUrlList.Shared;

namespace SimpleUrlList.SQL;

public class LinkRepository(string connectionString)
    : BaseRepository<Link>(connectionString), ILinkRepository
{
    public override async Task<PaginatedList<Link>> SearchAsync(int page, int pageSize, string query = "")
    {
        await using var connection = new SqlConnection(connectionString);
        var sql = "SELECT L.LinkId, L.Name, L.Url, G.LinkGroupId, G.Name, G.Description, G.ShortName, " +
                  "G.UserId,G.Clicked,G.CategoryId,G.CreatedAt FROM Links L " +
                  "JOIN LinkGroups G ON G.LinkGroupId = L.LinkId ";

        if (!string.IsNullOrEmpty(query))
            sql +=
                $"WHERE G.Name LIKE '%{query}%' OR G.Description LIKE '%{query}%' OR G.ShortName LIKE '%{query}%' OR L.Name LIKE '%{query}%'";

        var grid = await connection.QueryMultipleAsync(sql);
        var lookup = new Dictionary<string, Link>();
        grid.Read<Link, LinkGroup, Link>((link, linkGroup) =>
        {
            link.Group = linkGroup;
            if (!lookup.TryGetValue(link.LinkId, out _))
                lookup.Add(link.LinkId, link);
            return link;
        }, splitOn: "LinkGroupId");
        
        return PaginatedList<Link>.Create(lookup.Values.AsQueryable(), page, pageSize, query);
    }
}