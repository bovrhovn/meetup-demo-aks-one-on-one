using System.Data.SqlClient;
using Dapper;
using SimpleUrlList.Interfaces;
using SimpleUrlList.Models;

namespace SimpleUrlList.SQL
{
    public class CategoryRepository(string connectionString)
        : BaseRepository<Category>(connectionString), ICategoryRepository
    {
        public override async Task<Category> DetailsAsync(string entityId)
        {
            await using var connection = new SqlConnection(connectionString);
            var foundCategory = await connection.QuerySingleOrDefaultAsync<Category>(
                "SELECT C.CategoryId, C.Name FROM Category C WHERE C.CategoryId=@entityId", new {entityId});
            return foundCategory;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            await using var connection = new SqlConnection(connectionString);
            var categories = await connection.QueryAsync<Category>(
                "SELECT C.CategoryId, C.Name FROM Category C");
            return categories.ToList();
        }
    }
}