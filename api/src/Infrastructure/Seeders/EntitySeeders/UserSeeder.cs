using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Seeders.EntitySeeders;

public class UserSeeder : ISeeder
{
    public async Task SeedAsync(AppDbContext dbContext, IServiceProvider services)
    {
        if (await dbContext.Users.AnyAsync())
        {
            return;
        }

        dbContext.Users.AddRange(GetUsers());
        await dbContext.SaveChangesAsync();
    }

    private static List<User> GetUsers()
    {
        return
        [
            new User
            {
                Id = Guid.NewGuid(),
                Name = "Jonas",
                LastName = "Jonaitis"
            },
            new User
            {
                Id = Guid.NewGuid(),
                Name = "Ona",
                LastName = "Onaitė"
            },
            new User
            {
                Id = Guid.NewGuid(),
                Name = "Petras",
                LastName = "Petraitis"
            },
            new User
            {
                Id = Guid.NewGuid(),
                Name = "Rūta",
                LastName = "Rutkauskienė"
            }
        ];
    }
}