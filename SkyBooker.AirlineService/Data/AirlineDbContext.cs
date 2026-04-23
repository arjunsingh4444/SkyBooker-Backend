using Microsoft.EntityFrameworkCore;
using SkyBooker.AirlineService.Entities;

namespace SkyBooker.AirlineService.Data;

public class AirlineDbContext : DbContext
{
    public AirlineDbContext(DbContextOptions<AirlineDbContext> options) : base(options) { }

    public DbSet<Airline> Airlines => Set<Airline>();
    public DbSet<Airport> Airports => Set<Airport>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Airline>()
            .HasIndex(a => a.IataCode)
            .IsUnique();

        modelBuilder.Entity<Airport>()
            .HasIndex(a => a.IataCode)
            .IsUnique();

        base.OnModelCreating(modelBuilder);
    }
}
