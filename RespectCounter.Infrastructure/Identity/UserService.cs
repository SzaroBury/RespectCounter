using System.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RespectCounter.Domain.Contracts;
using RespectCounter.Domain.Model;

namespace RespectCounter.Infrastructure.Identity;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;

    public UserService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<User> GetByIdAsync(Guid id)
    {
        var appUser = await _userManager.FindByIdAsync(id.ToString())
            ?? throw new KeyNotFoundException($"User with the given ID {id} not found.");
        return appUser.ToDomain();
    }

    public async Task<User> GetByNameAsync(string username)
    {
        var appUser = await _userManager.FindByNameAsync(username)
            ?? throw new KeyNotFoundException($"User with the given username ({username}) was not found.");
        return appUser.ToDomain();
    }

    public async Task<User?> FindByNameAsync(string username)
    {
        AppUser? appUser = await _userManager.FindByNameAsync(username);

        if (appUser == null)
        {
            return null;
        }
        
        return appUser.ToDomain();
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        var appUser = await _userManager.FindByEmailAsync(email)
            ?? throw new KeyNotFoundException($"User with the given email ({email}) was not found.");
        return appUser.ToDomain();
    }

    public async Task<User?> FindByEmailAsync(string email)
    {
        AppUser? appUser = await _userManager.FindByEmailAsync(email);

        if (appUser == null)
        {
            return null;
        }
        
        return appUser.ToDomain();
    }

    public async Task<User> GetByRefreshTokenAsync(string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            throw new SecurityException("Invalid refreshToken");
        }

        var appUser = await _userManager.Users.FirstOrDefaultAsync(user => user.RefreshToken == refreshToken)
            ?? throw new SecurityException("Invalid refreshToken");

        if (appUser.RefreshTokenExpiration.HasValue
            && appUser.RefreshTokenExpiration < DateTime.Now)
        {
            appUser.RefreshToken = null;
            appUser.RefreshTokenExpiration = null;
        }

        return appUser.ToDomain();
    }

    public async Task<bool> CheckPasswordAsync(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
        {
            user = await _userManager.FindByEmailAsync(username);
            if(user == null)
            {
                throw new SecurityException($"{username} was not found.");
            }
        }  
        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<IList<string>> GetRolesAsync(User user)
    {
        var appUser = user.ToAppUser();
        return await _userManager.GetRolesAsync(appUser);
    }

    public async Task<User> CreateAsync(string userName, string password, string? email) //to-do: implement cancellationToken
    {
        var appUser = new AppUser
        {
            UserName = userName,
            Email = email
        };

        var result = await _userManager.CreateAsync(appUser, password); //to-do: implement cancellationToken

        if (!result.Succeeded)
        {
            throw new SecurityException(result.ToString());
        }

        return appUser.ToDomain();
    }

    public async Task SetUserRefreshTokenAsync(Guid userId, string refreshToken, DateTime refreshTokenExpiration)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString())
            ?? throw new KeyNotFoundException("User with the given ID was not found.");

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiration = refreshTokenExpiration;

        await _userManager.UpdateAsync(user);
    }
}
