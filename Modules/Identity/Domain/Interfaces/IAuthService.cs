using Identity.Application.DTOs;

namespace Identity.Domain.Interfaces;

public interface IAuthService
{
    Task RegisterAsync(RegisterDto dto);
    Task<string> LoginAsync(LoginDto dto);
}