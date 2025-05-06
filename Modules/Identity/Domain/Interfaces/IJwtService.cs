namespace Identity.Domain.Interfaces;

public interface IJwtService
{
    string GenerateToken(Domain.Entities.User user);
}