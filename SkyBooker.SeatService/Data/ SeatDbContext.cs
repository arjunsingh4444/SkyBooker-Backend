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

        base.OnModelCreating(modelBuilder);
    }
}