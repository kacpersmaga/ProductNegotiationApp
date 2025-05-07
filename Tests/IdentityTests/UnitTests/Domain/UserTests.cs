using Identity.Domain.Entities;

namespace Tests.IdentityTests.UnitTests.Domain;

public class UserTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithValidData()
    {
        var email = "user@example.com";
        var passwordHash = "hashedpassword123";

        var user = new User(email, passwordHash);

        Assert.NotEqual(Guid.Empty, user.Id);
        Assert.Equal(email, user.Email);
        Assert.Equal(passwordHash, user.PasswordHash);
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData(null)]
    public void SetEmail_ShouldThrow_WhenEmailIsNullOrWhiteSpace(string? invalidEmail)
    {
        var user = new User("valid@example.com", "hash");

        var ex = Assert.Throws<ArgumentException>(() => user.SetEmail(invalidEmail!));
        Assert.Equal("Email is required. (Parameter 'email')", ex.Message);
    }

    [Theory]
    [InlineData("invalid-email")]
    [InlineData("user@")]
    [InlineData("@domain.com")]
    [InlineData("user@domain")]
    public void SetEmail_ShouldThrow_WhenEmailIsInvalidFormat(string invalidEmail)
    {
        var user = new User("valid@example.com", "hash");

        var ex = Assert.Throws<ArgumentException>(() => user.SetEmail(invalidEmail));
        Assert.Equal("Invalid email format. (Parameter 'email')", ex.Message);
    }

    [Fact]
    public void SetEmail_ShouldUpdateEmail_WhenValid()
    {
        var user = new User("initial@example.com", "hash");
        var newEmail = "new@example.com";

        user.SetEmail(newEmail);

        Assert.Equal(newEmail, user.Email);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void SetPasswordHash_ShouldThrow_WhenNullOrWhitespace(string? passwordHash)
    {
        var user = new User("user@example.com", "initialHash");

        var ex = Assert.Throws<ArgumentException>(() => user.SetPasswordHash(passwordHash!));
        Assert.Equal("Password hash is required. (Parameter 'passwordHash')", ex.Message);
    }

    [Fact]
    public void SetPasswordHash_ShouldUpdatePasswordHash_WhenValid()
    {
        var user = new User("user@example.com", "initialHash");
        var newHash = "newPasswordHash";

        user.SetPasswordHash(newHash);

        Assert.Equal(newHash, user.PasswordHash);
    }
}