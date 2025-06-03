using RespectCounter.Domain.Model;

namespace RespectCounter.Infrastructure.Identity;

public static class UserMappingExtensions
{
    public static User ToDomain(this AppUser user)
    {
        return new User()
        {
            Id = user.Id,
            Username = user.UserName ?? "??",
            AvatarUrl = user.AvatarUrl,
            RefreshToken = user.RefreshToken,
            RefreshTokenExpiration = user.RefreshTokenExpiration,
            RecentlyBrowsedTags = user.RecentlyBrowsedTags?.ToList() ?? [],
            FavoriteTags = user.FavoriteTags?.ToList() ?? []
        };
    }

    public static AppUser ToAppUser(this User user)
    {
        return new AppUser()
        {
            Id = user.Id,
            UserName = user.Username,
            AvatarUrl = user.AvatarUrl,
            RefreshToken = user.RefreshToken,
            RefreshTokenExpiration = user.RefreshTokenExpiration,
            RecentlyBrowsedTags = user.RecentlyBrowsedTags?.ToList() ?? [],
            FavoriteTags = user.FavoriteTags?.ToList() ?? []
        };
    }
}