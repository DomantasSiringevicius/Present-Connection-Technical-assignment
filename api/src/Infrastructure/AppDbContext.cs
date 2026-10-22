using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    public DbSet<Room> Rooms { get; set; }

    public DbSet<Reservation> Reservations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Room>()
            .HasIndex(r => r.Name)
            .IsUnique();

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Organizer)
            .WithMany(u => u.Reservations)
            .HasForeignKey(r => r.OrganizerId);
        
        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Room)
            .WithMany(rm => rm.Reservations)
            .HasForeignKey(r => r.RoomId);
    }
}