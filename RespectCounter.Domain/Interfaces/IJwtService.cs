using Microsoft.AspNetCore.Identity;

namespace RespectCounter.Domain.Interfaces;

public interface IJwtService
{
    string GenerateToken(IdentityUser user, IList<string>? roles = null);
}