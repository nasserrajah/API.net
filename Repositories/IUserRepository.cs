using core_first.API.Models;

namespace core_first.API.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetUserByUsernameAsync(string username);
    }
}