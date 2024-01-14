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
        var sql = "SELECT U.UserId as SulUserId, U.FullName, U.Email, U.Password FROM Users U";
        var SulUsers = await connection.QueryAsync<SulUser>(sql);
        return SulUsers.ToList();
    }

    public override async Task<SulUser> InsertAsync(SulUser entity)
    {
        await using var connection = new SqlConnection(connectionString);
        entity.Password = PasswordHash.CreateHash(entity.Password);
        var item = await connection.ExecuteScalarAsync(
            $"INSERT INTO Users(FullName,Email,Password)VALUES(@{nameof(entity.FullName)},@{nameof(entity.Email)},@{nameof(entity.Password)});SELECT CAST(SCOPE_IDENTITY() as bigint)",
            entity);
        var userId = Convert.ToInt64(item);
        entity.UserId = userId.ToString();
        return entity;
    }

    public override async Task<SulUser> DetailsAsync(string entityId)
    {
        await using var connection = new SqlConnection(connectionString);
        var query = "SELECT U.UserId as SulUserId, U.FullName, U.Email, U.Password FROM Users U WHERE U.UserId=@entityId;" +
                    "SELECT T.* FROM WorkTasks T JOIN WorkTask2Tags FF on FF.WorkTaskId=T.WorkTaskId WHERE T.UserId=@entityId;" +
                    "SELECT F.* FROM UserSetting F WHERE F.UserId=@entityId;";

        var result = await connection.QueryMultipleAsync(query, new { entityId });
        var sulUser = await result.ReadSingleAsync<SulUser>();
        return sulUser;
    }

    public async Task<SulUser> LoginAsync(string username, string password)
    {
        await using var connection = new SqlConnection(connectionString);
        var item = await connection.QuerySingleOrDefaultAsync<SulUser>(
            "SELECT U.UserId, U.FullName, U.Email FROM Users U WHERE U.Email=@username", new { username });

        if (item == null) return null;

        item = await DetailsAsync(item.UserId);

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