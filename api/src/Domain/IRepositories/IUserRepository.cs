using Domain.Entities;

namespace Domain.IRepositories;

public interface IUserRepository
{
    Task<List<User>> GetUsersAsync();
}