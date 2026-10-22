using System.Text.Json.Serialization;
using Application.UseCases.Rooms.GetRooms;
using Infrastructure;
using Infrastructure.Persistence;
using Infrastructure.Seeders;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddInfrastructure();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetRoomsQuery).Assembly));

builder.Services
    .AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var seeder = scope.ServiceProvider.GetRequiredService<ApplicationSeeder>();
    
    await seeder.SeedAllAsync(dbContext, scope.ServiceProvider);
}

app.MapControllers();

app.Run();