using System.Text.Json.Serialization;
using Application.Options;
using Application.UseCases.Rooms.GetRooms;
using Application.Validators;
using Infrastructure;
using Infrastructure.Persistence;
using Infrastructure.Seeders;
using Presentation.ExceptionHandlers;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddInfrastructure();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetRoomsQuery).Assembly));
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.Configure<BookingOptions>(builder.Configuration.GetSection("Booking"));
builder.Services.AddScoped<BookingRulesValidator>();

builder.Services
    .AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

var app = builder.Build();
app.UseExceptionHandler();

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