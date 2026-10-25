using Domain.Entities;
using Domain.IRepositories;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository(AppDbContext dbContext) : IUserRepository
{
    public async Task<List<User>> GetUsersAsync()
    {
        return await dbContext.Users.Include(u => u.Reservations).ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        return await dbContext.Users.FirstOrDefaultAsync(u => u.Id.Equals(id));
    }
}