using System.Security.Claims;

namespace RespectCounter.API.Mappers;

public static class GuidMappingExtensions
{
    public static Guid ToGuid(this string id)
    {
        if (!Guid.TryParse(id, out Guid guid))
        {
            throw new FormatException($"The given ID ({id}) has invalid guid format.");
        }
        return guid;
    }

    public static Guid? ToNullableGuid(this string id)
    {
        if (!Guid.TryParse(id, out Guid guid))
        {
            return null;
        }
        return guid;
    }

    public static Guid? TryGetCurrentUserId(this ClaimsPrincipal user)
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "??";

        return userId.ToNullableGuid();
    }

    public static Guid GetCurrentUserId(this ClaimsPrincipal user)
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "??";

        return userId.ToGuid();
    }
}