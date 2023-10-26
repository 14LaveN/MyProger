using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using MyProger.Core.Entity;
using MyProger.Micro.Identity.Models.Identity;
using Microsoft.IdentityModel.Tokens;
using MyProger.Core.Entity.Account;

namespace MyProger.Micro.Identity.Extensions;

public static class JwtExtensions
{
    public static (string, DateTime) GenerateRefreshToken(this AppUser user, JwtOptions jwtOptions)
    {
        var refreshTokenExpireAt = DateTime.UtcNow.AddMinutes(jwtOptions.RefreshTokenExpire);
        var data = new RefreshTokenData
        {
            Expire = refreshTokenExpireAt, 
            UserId = user.Id, 
            Key = StringRandomizer.Randomize()
        };
        
        return (AesEncryptor.Encrypt(jwtOptions.Secret, JsonSerializer.Serialize(data)), refreshTokenExpireAt);
    }
    
    public static string GenerateAccessToken(this AppUser user, JwtOptions jwtOptions)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Secret.PadRight(64)));
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature);
        var tokeOptions = new JwtSecurityToken(
            issuer: jwtOptions.ValidIssuer,
            audience: jwtOptions.ValidAudience,
            claims: new List<Claim>
            {
                new (JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new (JwtRegisteredClaimNames.Name, user.UserName),
                new (JwtRegisteredClaimNames.Email, user.Email),
                new (JwtRegisteredClaimNames.GivenName, user.Firstname ?? string.Empty),
                new (JwtRegisteredClaimNames.FamilyName, user.Lastname ?? string.Empty),
                new ("middle_name", user.Patronymic ?? string.Empty),
                new (JwtRegisteredClaimNames.Birthdate, user.BirthDate?.ToString("dd.MM.yyyy") ?? string.Empty),
                new ("scope", user.Scopes == null || user.Scopes.Count == 0 
                    ? "" 
                    : string.Join(' ', user.Scopes.Select(x => x.Name)), "string", jwtOptions.ValidIssuer)
            },
            expires: DateTime.Now.AddMinutes(jwtOptions.Expire),
            signingCredentials: signinCredentials
        );
        
        return new JwtSecurityTokenHandler().WriteToken(tokeOptions);
    }
}