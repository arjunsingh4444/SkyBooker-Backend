using Microsoft.EntityFrameworkCore;
using SkyBooker.AuthService.Data;
using SkyBooker.AuthService.Entities;
using SkyBooker.AuthService.Interfaces;

namespace SkyBooker.AuthService.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AuthDbContext _context;

    public UserRepository(AuthDbContext context)
    {
        _context = context;
    }

    public async Task<User?> FindByEmail(string email)
        => await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

    public async Task<User?> FindByUserId(int id)
        => await _context.Users.FindAsync(id);

    public async Task<List<User>> GetAll()
        => await _context.Users.ToListAsync();

    public async Task Add(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task Update(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}