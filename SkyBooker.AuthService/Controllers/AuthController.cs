using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkyBooker.AuthService.DTOs;
using SkyBooker.AuthService.Interfaces;

namespace SkyBooker.AuthService.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service;
    }

    // REGISTER
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        await _service.Register(dto);
        return Ok("User registered successfully");
    }

    // LOGIN
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var token = await _service.Login(dto);
        return Ok(new { token });
    }

    //GET PROFILE
    [Authorize]
    [HttpGet("profile/{id}")]
    public async Task<IActionResult> GetProfile(int id)
    {
        var user = await _service.GetProfile(id);
        return Ok(user);
    }

    // UPDATE PROFILE
    [Authorize]
    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile(UpdateProfileDto dto)
    {
        await _service.UpdateProfile(dto);
        return Ok("Profile updated");
    }

    // CHANGE PASSWORD
    [Authorize]
    [HttpPut("password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
    {
        await _service.ChangePassword(dto);
        return Ok("Password changed");
    }

    //DEACTIVATE ACCOUNT
    [Authorize]
    [HttpDelete("deactivate/{id}")]
    public async Task<IActionResult> Deactivate(int id)
    {
        await _service.Deactivate(id);
        return Ok("Account deactivated");
    }

    // GET ALL USERS (ADMIN ONLY)
    [Authorize(Roles = "ADMIN")]
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _service.GetUsers();
        return Ok(users);
    }
}