using Microsoft.EntityFrameworkCore;
using SkyBooker.BookingService.Entities;

namespace SkyBooker.BookingService.Data;

public class BookingDbContext : DbContext
{
    public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options) {}

    public DbSet<Booking> Bookings { get; set; }
}