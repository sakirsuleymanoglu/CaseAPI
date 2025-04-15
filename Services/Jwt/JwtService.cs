using CaseAPI.Abstractions.Jwt;
using CaseAPI.Entities;
using CaseAPI.Models.Jwt;
using CaseAPI.Options.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CaseAPI.Services.Jwt;

public sealed class JwtService(IOptions<JwtOptions> options,
    UserManager<AppUser> userManager) : IJwtService
{
    public async Task<CreatedJwt> CreateAsync(AppUser user)
    {
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.SecurityKey));

        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        List<Claim> claims =
        [
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.NameIdentifier, user.Id.ToString())
        ];

        var roles = await userManager.GetRolesAsync(user);

        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        DateTime notBefore = DateTime.Now;
        DateTime expires = notBefore.AddMinutes(30);

        JwtSecurityToken token = new(
            issuer: options.Value.Issuer,
            audience: options.Value.Audience,
            notBefore: notBefore,
            claims: claims,
            expires: expires,
            signingCredentials: signingCredentials);

        return new()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expires
        };

    }
}
