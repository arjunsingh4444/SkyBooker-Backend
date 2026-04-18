using Microsoft.EntityFrameworkCore;
using SkyBooker.AuthService.Data;
using SkyBooker.AuthService.Entities;
using SkyBooker.AuthService.Interfaces;

namespace SkyBooker.AuthService.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UsersDbContext _context;

    public UserRepository(UsersDbContext context)
    {
        _context = context;
    }

    public async Task<User?> FindByEmail(string email)
        => await _context.Users.FirstOrDefaultAsync(x => x.Email == email);

    public async Task<User?> FindByUserId(int id)
        => await _context.Users.FindAsync(id);

    public async Task<bool> ExistsByEmail(string email)
        => await _context.Users.AnyAsync(x => x.Email == email);

    public async Task<List<User>> FindAllByRole(string role)
        => await _context.Users.Where(x => x.Role == role).ToListAsync();

    public async Task AddUser(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUser(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}