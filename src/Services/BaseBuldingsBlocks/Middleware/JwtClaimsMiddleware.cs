using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BaseBuldingsBlocks.Middleware;

public class JwtClaimsMiddleware(IConfiguration configuration)
    : IMiddleware
{
    public bool IsAuthorized { get; private set; } = false;
    public bool IsValidToken { get; private set; } = false;
    public string? Email { get; private set; }
    public string? UserName { get; private set; }
    public string? Role { get; private set; }


    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

        if (string.IsNullOrEmpty(token))
        {
            await next(context);
            return;
        }

        IsAuthorized = true;

        var claims = await ValidateTokenAsync(token);

        if (claims is null)
        {
            await next(context);
            return;
        }

        IsValidToken = true;
        Email = GetClaimValueOrDefault(claims, ClaimTypes.Email);
        Role = GetClaimValueOrDefault(claims, ClaimTypes.Role);
        UserName = GetClaimValueOrDefault(claims, ClaimTypes.Name);

        await next(context);
    }

    private async Task<IDictionary<string, object>?> ValidateTokenAsync(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityKey = Encoding.UTF8.GetBytes(configuration["JwtSettings:ActiveToken:SecurityKey"]!);

        var validationResult = await tokenHandler.ValidateTokenAsync(token,
            new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(securityKey),
                ValidateAudience = false,
                ValidateIssuer = false
            });

        if (validationResult.IsValid)
            return validationResult.Claims;

        return null;
    }

    public static string? GetClaimValueOrDefault(IDictionary<string, object> claims, string key)
    {
        if(claims.TryGetValue(key, out var strObject) && strObject is string str)
        {
            return str;
        }

        return null;
    }
}
