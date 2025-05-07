using System.Text.RegularExpressions;

namespace Identity.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    
    private static readonly Regex EmailRegex = new Regex(
        @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", 
        RegexOptions.Compiled);
    
    private User() { }

    public User(string email, string passwordHash)
    {
        Id = Guid.NewGuid();
        SetEmail(email);
        SetPasswordHash(passwordHash);
    }

    public void SetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.", nameof(email));
        
        if (!IsValidEmail(email))
            throw new ArgumentException("Invalid email format.", nameof(email));

        Email = email;
    }

    public void SetPasswordHash(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password hash is required.", nameof(passwordHash));
        
        PasswordHash = passwordHash;
    }
    
    private bool IsValidEmail(string email)
    {
        return EmailRegex.IsMatch(email);
    }
}