using System.Data.SqlClient;
using Dapper;
using SimpleUrlList.Interfaces;
using SimpleUrlList.Models;
using SimpleUrlList.Shared;

namespace SimpleUrlList.SQL;

public class SulUserService(string connectionString) : BaseRepository<SulUser>(connectionString), IUserService
{
    public override async Task<List<SulUser>> GetAsync()
    {
        await using var connection = new SqlConnection(connectionString);
        var sql = "SELECT U.UserId, U.FullName, U.Email, U.Password FROM Users U";
        var SulUsers = await connection.QueryAsync<SulUser>(sql);
        return SulUsers.ToList();
    }

    public override async Task<SulUser> InsertAsync(SulUser entity)
    {
        await using var connection = new SqlConnection(connectionString);
        entity.Password = PasswordHash.CreateHash(entity.Password);
        entity.UserId = Guid.NewGuid();
        var itemAffected = await connection.ExecuteAsync(
            "INSERT INTO Users(UserId, FullName,Email,Password)VALUES(@uid,@fn,@em,@pwd)",
            new
            {
                uid = entity.UserId,
                fn = entity.FullName,
                em = entity.Email,
                pwd = entity.Password
            });
        return entity;
    }

    public override async Task<SulUser> DetailsAsync(string entityId)
    {
        await using var connection = new SqlConnection(connectionString);
        var query =
            "SELECT U.UserId, U.FullName, U.Email, U.Password FROM Users U WHERE U.UserId=@entityId;" +
            "SELECT G.LinkGroupId, G.Name, G.Description, G.ShortName, G.UserId,G.Clicked,G.CategoryId," +
            "G.CreatedAt, C.CategoryId, C.Name FROM LinkGroups G JOIN Categories C on C.CategoryId=G.CategoryId " +
            "WHERE G.UserId=@entityId";

        var result = await connection.QueryMultipleAsync(query, new { entityId });
        var sulUser = await result.ReadSingleAsync<SulUser>();
        var linkGroupUsers = await result.ReadAsync<LinkGroup>();
        sulUser.Groups = linkGroupUsers.ToList();
        return sulUser;
    }

    public async Task<SulUser?> LoginAsync(string username, string password)
    {
        await using var connection = new SqlConnection(connectionString);
        var item = await connection.QuerySingleOrDefaultAsync<SulUser>(
            "SELECT U.UserId, U.FullName, U.Email FROM Users U WHERE U.Email=@username", new { username });

        if (item == null) return null;

        item = await DetailsAsync(item.UserId.ToString());

        var validateHash = PasswordHash.ValidateHash(password, item.Password);
        return validateHash ? item : null;
    }

    public async Task<SulUser> FindAsync(string email)
    {
        await using var connection = new SqlConnection(connectionString);
        var sulUsers = await connection.QueryAsync<SulUser>(
            "SELECT U.UserId, U.FullName, U.Email FROM Users U WHERE U.Email=@email", new { email });
        return sulUsers.Any() ? sulUsers.ElementAt(0) : null;
    }
}