using Microsoft.EntityFrameworkCore;
using core_first.Application.Interfaces.Repositories;
using core_first.Domain.Entities;
using core_first.Infrastructure.Persistence;

namespace core_first.Infrastructure.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context) { }

    public async Task<User?> GetByUsernameAsync(string username)
        => await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
}