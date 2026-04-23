using Microsoft.AspNetCore.Identity;
using SkyBooker.AuthService.DTOs;
using SkyBooker.AuthService.Entities;
using SkyBooker.AuthService.Helpers;
using SkyBooker.AuthService.Interfaces;

namespace SkyBooker.AuthService.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _repo;
    private readonly JwtHelper _jwt;
    private readonly PasswordHasher<User> _hasher = new();

    public AuthService(IUserRepository repo, JwtHelper jwt)
    {
        _repo = repo;
        _jwt = jwt;
    }

    public async Task Register(RegisterDto dto)
    {
        var exists = await _repo.FindByEmail(dto.Email);
        if (exists != null) throw new Exception("User exists");

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            Role = "USER"
        };

        user.PasswordHash = _hasher.HashPassword(user, dto.Password);
        await _repo.Add(user);
    }

    public async Task<string> Login(LoginDto dto)
    {
        var user = await _repo.FindByEmail(dto.Email);
        if (user == null) throw new Exception("Invalid credentials");

        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
        if (result == PasswordVerificationResult.Failed)
            throw new Exception("Invalid credentials");

        return _jwt.GenerateToken(user.Email, user.Role ?? "USER");
    }

    public async Task<User?> GetProfile(int id)
        => await _repo.FindByUserId(id);

    public async Task UpdateProfile(UpdateProfileDto dto)
    {
        var user = await _repo.FindByUserId(dto.UserId);
        if (user == null) return;

        user.FullName = dto.FullName;
        user.Phone = dto.Phone;

        await _repo.Update(user);
    }

    public async Task ChangePassword(ChangePasswordDto dto)
    {
        var user = await _repo.FindByUserId(dto.UserId);
        if (user == null) return;

        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.OldPassword);
        if (result == PasswordVerificationResult.Failed)
            throw new Exception("Wrong password");

        user.PasswordHash = _hasher.HashPassword(user, dto.NewPassword);
        await _repo.Update(user);
    }

    public async Task Deactivate(int id)
    {
        var user = await _repo.FindByUserId(id);
        if (user == null) return;

        user.IsActive = false;
        await _repo.Update(user);
    }

    public async Task<List<User>> GetUsers()
        => await _repo.GetAll();
}