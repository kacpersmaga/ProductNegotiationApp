using Identity.Application.DTOs;
using Identity.Application.Services;
using Identity.Domain.Entities;
using Identity.Domain.Interfaces;
using Moq;

namespace Tests.IdentityTests.UnitTests.Application.Services;

public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepoMock = new();
    private readonly Mock<IJwtService> _jwtServiceMock = new();
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _authService = new AuthService(_userRepoMock.Object, _jwtServiceMock.Object);
    }

    [Fact]
    public async Task RegisterAsync_ShouldAddUser_WhenEmailDoesNotExist()
    {
        // Arrange
        var dto = new RegisterDto { Email = "new@example.com", Password = "Password123" };
        _userRepoMock.Setup(r => r.GetByEmailAsync(dto.Email)).ReturnsAsync((User?)null);

        // Act
        await _authService.RegisterAsync(dto);

        // Assert
        _userRepoMock.Verify(r => r.AddAsync(It.Is<User>(u => u.Email == dto.Email)), Times.Once);
    }

    [Fact]
    public async Task RegisterAsync_ShouldThrow_WhenEmailAlreadyExists()
    {
        // Arrange
        var dto = new RegisterDto { Email = "existing@example.com", Password = "Password123" };
        _userRepoMock.Setup(r => r.GetByEmailAsync(dto.Email)).ReturnsAsync(new User(dto.Email, "hash"));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ApplicationException>(() => _authService.RegisterAsync(dto));
        Assert.Equal("User already exists.", ex.Message);
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnToken_WhenCredentialsAreValid()
    {
        // Arrange
        var dto = new LoginDto { Email = "valid@example.com", Password = "Password123" };
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        var user = new User(dto.Email, passwordHash);

        _userRepoMock.Setup(r => r.GetByEmailAsync(dto.Email)).ReturnsAsync(user);
        _jwtServiceMock.Setup(j => j.GenerateToken(user)).Returns("mock-jwt-token");

        // Act
        var token = await _authService.LoginAsync(dto);

        // Assert
        Assert.Equal("mock-jwt-token", token);
    }

    [Fact]
    public async Task LoginAsync_ShouldThrow_WhenUserNotFound()
    {
        // Arrange
        var dto = new LoginDto { Email = "unknown@example.com", Password = "Password123" };
        _userRepoMock.Setup(r => r.GetByEmailAsync(dto.Email)).ReturnsAsync((User?)null);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ApplicationException>(() => _authService.LoginAsync(dto));
        Assert.Equal("Invalid credentials.", ex.Message);
    }

    [Fact]
    public async Task LoginAsync_ShouldThrow_WhenPasswordIsInvalid()
    {
        // Arrange
        var dto = new LoginDto { Email = "valid@example.com", Password = "WrongPassword" };
        var user = new User(dto.Email, BCrypt.Net.BCrypt.HashPassword("CorrectPassword"));

        _userRepoMock.Setup(r => r.GetByEmailAsync(dto.Email)).ReturnsAsync(user);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ApplicationException>(() => _authService.LoginAsync(dto));
        Assert.Equal("Invalid credentials.", ex.Message);
    }
}