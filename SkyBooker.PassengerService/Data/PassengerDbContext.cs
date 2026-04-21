using Microsoft.EntityFrameworkCore;
using SkyBooker.PassengerService.Entities;

namespace SkyBooker.PassengerService.Data;

public class PassengerDbContext : DbContext
{
    public PassengerDbContext(DbContextOptions<PassengerDbContext> options) : base(options) {}

    public DbSet<Passenger> Passengers => Set<Passenger>();
}