using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Security.Claims;
using UserService.Models;
using UserService.Dtos;
using System.Text;

namespace UserService.UseCases.Services.Authentication;

public class AuthenticateManager(IOptions<JwtTokensSettings> jwtTokensSettingsOptions,
    UserManager<User> userManager) 
    : IAuthenticateManager
{
    private readonly JwtTokensSettings jwtTokensSettings = jwtTokensSettingsOptions.Value;


    public async Task<BaerarTokenDto> GenerateTokensAsync(User user)
    {
        var activeToken = await CreateActiveTokenAsync(user);
        var refreshToken = await CreateRefreshTokenAsync();

        return new BaerarTokenDto { Token = new JwtSecurityTokenHandler().WriteToken(activeToken), 
            RefreshToken = refreshToken,
            ExpireRefreshToken = DateTime.UtcNow.AddMinutes(jwtTokensSettings.RefreshToken.Expire)};
    }

    private async Task<JwtSecurityToken> CreateActiveTokenAsync(User user)
    {
        var claims = await GetUserClaimsAsync(user);
        var signinCredentials = GetSigninCredentials();

        return new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(jwtTokensSettings.ActiveToken.Expire),
            signingCredentials: signinCredentials
            );
    }

    private Task<string> CreateRefreshTokenAsync()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Task.FromResult(Convert.ToBase64String(randomNumber)); 
    }

    private async Task<IEnumerable<Claim>> GetUserClaimsAsync(User user)
    {
        if (user.UserName is null || user.Email is null)
            throw new Exception("Jwt token cannot be create");

        var roles = await userManager.GetRolesAsync(user);

        return [
            new (ClaimTypes.Name, user.UserName),
            new (ClaimTypes.Email, user.Email),
            new (ClaimTypes.Role, roles.First()),
            new (ClaimTypes.NameIdentifier, user.Id)
        ];
    }

    private SigningCredentials GetSigninCredentials()
    {
        var securityKey = jwtTokensSettings.ActiveToken.SecurityKey;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey!));

        return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    }
}

