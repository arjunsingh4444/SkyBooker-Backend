using Microsoft.EntityFrameworkCore;
using SkyBooker.NotificationService.Entities;

namespace SkyBooker.NotificationService.Data;

public class NotificationDbContext : DbContext
{
    public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options) { }

    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Notification>()
            .HasIndex(n => n.RecipientId);

        modelBuilder.Entity<Notification>()
            .HasIndex(n => n.RelatedBookingId);

        base.OnModelCreating(modelBuilder);
    }
}
