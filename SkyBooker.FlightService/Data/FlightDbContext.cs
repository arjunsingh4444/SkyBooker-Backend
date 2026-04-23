using Microsoft.EntityFrameworkCore;
using SkyBooker.FlightService.Entities;

namespace SkyBooker.FlightService.Data;

public class FlightDbContext : DbContext
{
    public FlightDbContext(DbContextOptions<FlightDbContext> options)
        : base(options) { }

    public DbSet<Flight> Flights => Set<Flight>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Flight>()
            .HasIndex(f => f.FlightNumber)
            .IsUnique();

        modelBuilder.Entity<Flight>()
            .HasIndex(f => new { f.OriginAirportCode, f.DestinationAirportCode, f.DepartureTime });

        base.OnModelCreating(modelBuilder);
    }
}