using Identity.Application.DTOs;
using Identity.Domain.Entities;
using Identity.Domain.Interfaces;

namespace Identity.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepo;
    private readonly IJwtService     _jwtService;

    public AuthService(IUserRepository userRepo, IJwtService jwtService)
    {
        _userRepo   = userRepo;
        _jwtService = jwtService;
    }

    public async Task RegisterAsync(RegisterDto dto)
    {
        if (await _userRepo.GetByEmailAsync(dto.Email) != null)
            throw new ApplicationException("User already exists.");

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        var user = new User(dto.Email, passwordHash);
        await _userRepo.AddAsync(user);
    }

    public async Task<string> LoginAsync(LoginDto dto)
    {
        var user = await _userRepo.GetByEmailAsync(dto.Email)
                   ?? throw new ApplicationException("Invalid credentials.");

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new ApplicationException("Invalid credentials.");

        return _jwtService.GenerateToken(user);
    }
}