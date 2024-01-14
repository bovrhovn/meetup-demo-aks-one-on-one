using System.Data.SqlClient;
using Dapper;
using SimpleUrlList.Interfaces;
using SimpleUrlList.Models;
using SimpleUrlList.Shared;

namespace SimpleUrlList.SQL;

public class LinkGroupRepository(string connectionString)
    : BaseRepository<LinkGroup>(connectionString), ILinkGroupRepository
{
    public override async Task<LinkGroup> InsertAsync(LinkGroup entity)
    {
        await using var connection = new SqlConnection(connectionString);
        entity.CreatedAt = DateTime.Now;
        var sql =
            $"INSERT INTO LinkGroups(Name, Description,ShortName,UserId,Clicked,CategoryId,CreatedAt)" +
            $"VALUES (@name,@desc,@shortName,@userId,@clicked,@catId,@created);" +
            "SELECT SCOPE_IDENTITY()";
        var item = await connection.ExecuteScalarAsync(sql, new
        {
            name = entity.Name,
            desc = entity.Description,
            shortName = entity.ShortName,
            userId = entity.User.UserId,
            clicked = 0,
            catId = entity.Category.CategordId,
            created = entity.CreatedAt
        });
        
        entity.LinkGroupId = item.ToString() ?? string.Empty;

        if (entity.Links != null && entity.Links.Any())
            await SaveLinksToDatabaseAsync(entity.LinkGroupId, entity.Links.ToArray());

        return entity;
    }
    
    private async Task SaveLinksToDatabaseAsync(string linkGroupId, Link[] links)
    {
        await using var connection = new SqlConnection(connectionString);
        var sql =
            $"INSERT INTO Links(Name, Url,LinkGroupId)" +
            $"VALUES (@name,@url,@lgId)";
        foreach (var link in links)
        {
            await connection.ExecuteAsync(sql, new
            {
                name = link.Name,
                url = link.Url,
                lgId = linkGroupId
            });
        }
    }

    public override async Task<PaginatedList<LinkGroup>> SearchAsync(int page, int pageSize, string query = "")
    {
        await using var connection = new SqlConnection(connectionString);
        var sql = "SELECT G.LinkGroupId, G.Name, G.Description, G.ShortName, " +
                  "G.UserId,G.Clicked,G.CategoryId,G.CreatedAt, C.CategoryId, C.Name FROM LinkGroups G " +
                  "JOIN Categories C ON G.CategoryId = C.CategoryId ";

        if (!string.IsNullOrEmpty(query))
            sql +=
                $"WHERE G.Name LIKE '%{query}%' OR G.Description LIKE '%{query}%' OR G.ShortName LIKE '%{query}%' OR C.Name LIKE '%{query}%'";

        var grid = await connection.QueryMultipleAsync(sql);
        var lookup = new Dictionary<string, LinkGroup>();
        grid.Read<LinkGroup, Category, LinkGroup>((linkGroup, category) =>
        {
            linkGroup.Category = category;
            if (!lookup.TryGetValue(linkGroup.LinkGroupId, out _))
                lookup.Add(linkGroup.LinkGroupId, linkGroup);
            return linkGroup;
        }, splitOn: "CategoryId");
        
        return PaginatedList<LinkGroup>.Create(lookup.Values.AsQueryable(), page, pageSize, query);
    }
    
    public override async Task<LinkGroup> DetailsAsync(string entityId)
    {
        await using var connection = new SqlConnection(connectionString);
        var query =
            "SELECT G.LinkGroupId, G.Name, G.Description, G.ShortName, G.UserId,G.Clicked,G.CategoryId,G.CreatedAt " +
            "FROM LinkGroups G WHERE G.LinkGroupId=@entityId;" +
            "SELECT P.CategoryId, P.Name FROM Categories P " +
            "JOIN LinkGroups S on S.CategoryId=P.CategoryId WHERE S.LinkGroupId=@entityId;" +
            "SELECT L.LinkId, L.Name, L.Url, L.Description, L.ShortName, L.UserId, L.Clicked, L.LinkGroupId, L.CreatedAt " +
            "FROM Links L JOIN LinkGroups S on S.LinkGroupId=L.LinkGroupId WHERE S.LinkGroupId=@entityId";

        var result = await connection.QueryMultipleAsync(query, new { entityId });
        var linkGroup = await result.ReadSingleAsync<LinkGroup>();
        var category = await result.ReadSingleAsync<Category>();
        var links = await result.ReadAsync<Link>();
        linkGroup.Category = category;
        linkGroup.Links = links.ToList();
        return linkGroup;
    }

    public async Task<List<LinkGroup>> GetFromSpecificUserAsync(string userId)
    {
        await using var connection = new SqlConnection(connectionString);
        var sql = "SELECT G.LinkGroupId, G.Name, G.Description, G.ShortName, " +
                  "G.UserId,G.Clicked,G.CategoryId,G.CreatedAt, C.CategoryId, C.Name FROM LinkGroups G " +
                  "JOIN Categories C ON G.CategoryId = C.CategoryId WHERE G.UserId=@userId";

        var grid = await connection.QueryMultipleAsync(sql, new {userId});
        var lookup = new Dictionary<string, LinkGroup>();
        grid.Read<LinkGroup, Category, LinkGroup>((linkGroup, category) =>
        {
            linkGroup.Category = category;
            if (!lookup.TryGetValue(linkGroup.LinkGroupId, out _))
                lookup.Add(linkGroup.LinkGroupId, linkGroup);
            return linkGroup;
        }, splitOn: "CategoryId");
        return lookup.Values.ToList();
    }

    public override async Task<List<LinkGroup>> GetAsync()
    {
        await using var connection = new SqlConnection(connectionString);
        var sql =
            "SELECT G.LinkGroupId, G.Name, G.Description, G.ShortName, " +
            "G.UserId,G.Clicked,G.CategoryId,G.CreatedAt, C.CategoryId, C.Name FROM LinkGroups G " +
            "JOIN Categories C ON G.CategoryId = C.CategoryId ";

        var grid = await connection.QueryMultipleAsync(sql);
        var lookup = new Dictionary<string, LinkGroup>();
        grid.Read<LinkGroup, Category, LinkGroup>((linkGroup, category) =>
        {
            linkGroup.Category = category;
            if (!lookup.TryGetValue(linkGroup.LinkGroupId, out _))
                lookup.Add(linkGroup.LinkGroupId, linkGroup);
            return linkGroup;
        }, splitOn: "CategoryId");
        return lookup.Values.ToList();
    }

    public override async Task<bool> DeleteAsync(string entityId)
    {
        await using var connection = new SqlConnection(connectionString);
        var sql = "DELETE FROM Links WHERE LinkGroupId=@linkGroupId";
        var resultAffected = await connection.ExecuteAsync(sql, new
        {
            linkGroupId = entityId
        });

        sql = "DELETE FROM LinkGroups WHERE LinkGroupId=@linkGroupId";
        resultAffected = await connection.ExecuteAsync(sql, new
        {
            linkGroupId = entityId
        });

        return resultAffected > 0;
    }
}