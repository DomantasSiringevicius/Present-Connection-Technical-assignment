using Infrastructure.Persistence;

namespace Infrastructure.Seeders;

public class ApplicationSeeder(IEnumerable<ISeeder> seeders)
{
    public async Task SeedAllAsync(AppDbContext context, IServiceProvider services)
    {
        foreach (var seeder in seeders)
        {
            await seeder.SeedAsync(context, services);
        }
    }
}