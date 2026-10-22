using Infrastructure.Persistence;

namespace Infrastructure.Seeders;

public interface ISeeder
{
    Task SeedAsync(AppDbContext context, IServiceProvider services);
}