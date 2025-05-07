using Identity.Application.DTOs;
using Identity.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.API.Common;

namespace Identity.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService auth, ILogger<AuthController> logger)
    {
        _auth = auth;
        _logger = logger;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        _logger.LogInformation("Registration attempt for email: {Email}", dto.Email);
        
        await _auth.RegisterAsync(dto);
        
        _logger.LogInformation("User registered successfully: {Email}", dto.Email);
        return Ok(ApiResponse<string>.Ok("Registration successful"));
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        _logger.LogInformation("Login attempt for email: {Email}", dto.Email);
        
        var token = await _auth.LoginAsync(dto);
        
        _logger.LogInformation("User logged in successfully: {Email}", dto.Email);
        return Ok(ApiResponse<object>.Ok(new { Token = token }));
    }
}