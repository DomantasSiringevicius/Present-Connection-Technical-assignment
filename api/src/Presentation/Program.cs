using Infrastructure;
using Infrastructure.Persistence;
using Infrastructure.Seeders;
using Infrastructure.Seeders.EntitySeeders;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddInfrastructure();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var seeder = scope.ServiceProvider.GetRequiredService<ApplicationSeeder>();
    
    await seeder.SeedAllAsync(dbContext, app.Services);
}

app.MapControllers();

app.Run();