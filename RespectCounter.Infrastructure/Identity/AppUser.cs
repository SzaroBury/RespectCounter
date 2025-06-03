using Microsoft.AspNetCore.Identity;
using RespectCounter.Domain.Model;

namespace RespectCounter.Infrastructure.Identity;

public class AppUser : IdentityUser<Guid>
{
    public string? AvatarUrl { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiration { get; set; }
    public virtual ICollection<Tag> RecentlyBrowsedTags { get; set; } = [];
    public virtual ICollection<Tag> FavoriteTags { get; set; } = [];
}