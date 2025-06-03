using RespectCounter.Domain.Model;

namespace RespectCounter.Domain.Contracts;

public interface IUserService
{
    Task<User> GetByIdAsync(Guid id);
    Task<User> GetByNameAsync(string username);
    Task<User?> FindByNameAsync(string username);
    Task<User> GetByEmailAsync(string email);
    Task<User?> FindByEmailAsync(string email);
    Task<User> GetByRefreshTokenAsync(string refreshToken);
    Task<bool> CheckPasswordAsync(string username, string password);
    Task<IList<string>> GetRolesAsync(User user);
    Task<User> CreateAsync(string userName, string password, string? email);
    Task SetUserRefreshTokenAsync(Guid userId, string refreshToken, DateTime refreshTokenExpiration);
}