using Domain.Entities;

namespace Domain.IRepositories;

public interface IUserRepository
{
    Task<List<User>> GetUsersAsync();
    
    Task<User?> GetUserByIdAsync(Guid id);
}