using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RespectCounter.Domain.Interfaces;

public class JwtService: IJwtService
{
    private readonly IConfiguration config;

    public JwtService(IConfiguration config)
    {
        this.config = config;
    }

    public string GenerateToken(IdentityUser user, IList<string>? roles = null)
    {
        if(user == null || user.UserName == null || user.Email == null)
        {
            throw new NullReferenceException("User cannot be null.");
        }

        var key = config["Jwt:Key"];
        if(key == null)
        {
            throw new NullReferenceException("JWT key cannot be null.");
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName),
        };

        if(roles != null) 
        {
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        }

        var encodedKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var creds = new SigningCredentials(encodedKey, SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(config["Jwt:ExpireMinutes"])),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
