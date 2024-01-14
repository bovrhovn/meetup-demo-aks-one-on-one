using SimpleUrlList.Models;

namespace SimpleUrlList.Interfaces;

public interface IUserService
{
    Task<SulUser> LoginAsync(string username, string password);
    Task<SulUser> FindAsync(string email);
    Task<SulUser> InsertAsync(SulUser newUser);
}