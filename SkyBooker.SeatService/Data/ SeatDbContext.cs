using Microsoft.EntityFrameworkCore;
using SkyBooker.SeatService.Entities;

namespace SkyBooker.SeatService.Data;

public class SeatDbContext : DbContext
{
    public SeatDbContext(DbContextOptions<SeatDbContext> options) : base(options) {}

    public DbSet<Seat> Seats => Set<Seat>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Seat>()
            .HasIndex(s => new { s.FlightId, s.SeatNumber })
            .IsUnique();

        modelBuilder.Entity<Seat>()
            .Property(s => s.PriceMultiplier)
            .HasColumnType("decimal(18,2)");

        base.OnModelCreating(modelBuilder);
    }
}