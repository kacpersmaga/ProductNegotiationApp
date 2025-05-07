using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Identity.Application.Services;
using Identity.Domain.Entities;
using Identity.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Tests.IdentityTests.UnitTests.Application.Services;

public class JwtServiceTests
{
    private readonly JwtSettings _jwtSettings = new()
    {
        Secret = "this_is_a_very_secure_256bit_secret_key_!!",
        Issuer = "TestIssuer",
        Audience = "TestAudience",
        ExpiryMinutes = 60
    };

    private readonly JwtService _jwtService;

    public JwtServiceTests()
    {
        var options = Options.Create(_jwtSettings);
        _jwtService = new JwtService(options);
    }

    [Fact]
    public void GenerateToken_ShouldReturnValidJwt()
    {
        // Arrange
        var user = new User("user@example.com", "passwordHash");

        // Act
        var token = _jwtService.GenerateToken(user);

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(token));
        
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        Assert.Equal(_jwtSettings.Issuer, jwt.Issuer);
        Assert.Equal(_jwtSettings.Audience, jwt.Audiences.First());

        Assert.Contains(jwt.Claims, c => c.Type == JwtRegisteredClaimNames.Sub && c.Value == user.Id.ToString());
        Assert.Contains(jwt.Claims, c => c.Type == JwtRegisteredClaimNames.Email && c.Value == user.Email);
    }

    [Fact]
    public void GenerateToken_ShouldExpireCorrectly()
    {
        // Arrange
        var user = new User("user@example.com", "passwordHash");

        // Act
        var token = _jwtService.GenerateToken(user);
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);

        // Assert
        var expectedExpiry = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes);
        Assert.True(jwt.ValidTo <= expectedExpiry.AddSeconds(5));
    }

    [Fact]
    public void GenerateToken_ShouldBeSignedCorrectly()
    {
        // Arrange
        var user = new User("user@example.com", "passwordHash");

        // Act
        var token = _jwtService.GenerateToken(user);
        var handler = new JwtSecurityTokenHandler();

        var parameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
            ValidateLifetime = false
        };

        // Assert
        handler.ValidateToken(token, parameters, out _);
    }
}