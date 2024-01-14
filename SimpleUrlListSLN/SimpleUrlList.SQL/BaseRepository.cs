using SimpleUrlList.Interfaces;
using SimpleUrlList.Shared;

namespace SimpleUrlList.SQL;

public abstract class BaseRepository<TEntity>(string connectionString) : IDataRepository<TEntity>
    where TEntity : class
{
    public virtual Task<PaginatedList<TEntity>> SearchAsync(int page, int pageSize, string query = "") =>
        throw new NotImplementedException();

    public virtual Task<PaginatedList<TEntity>> GetAsync(int page, int pageSize) => throw new NotImplementedException();

    public virtual Task<List<TEntity>> GetAsync() => throw new NotImplementedException();

    public virtual Task<bool> DeleteAsync(string entityId) => throw new NotImplementedException();

    public virtual Task<bool> UpdateAsync(TEntity entity) => throw new NotImplementedException();

    public virtual Task<TEntity> InsertAsync(TEntity entity) => throw new NotImplementedException();

    public virtual Task<TEntity> DetailsAsync(string entityId) => throw new NotImplementedException();
}