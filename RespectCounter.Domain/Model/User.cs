namespace RespectCounter.Domain.Model;

public class User : Entity
{
    public string Username { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiration { get; set; }
    public virtual ICollection<Tag> RecentlyBrowsedTags { get; set; } = [];
    public virtual ICollection<Tag> FavoriteTags { get; set; } = [];
}