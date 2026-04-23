using Microsoft.EntityFrameworkCore;
using SkyBooker.AuthService.Entities;

namespace SkyBooker.AuthService.Data;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
}