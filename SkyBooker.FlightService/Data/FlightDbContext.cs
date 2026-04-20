using Microsoft.EntityFrameworkCore;
using SkyBooker.FlightService.Entities;

namespace SkyBooker.FlightService.Data;

public class FlightDbContext : DbContext
{
    public FlightDbContext(DbContextOptions<FlightDbContext> options) : base(options) {}

    public DbSet<Flight> Flights { get; set; }
}