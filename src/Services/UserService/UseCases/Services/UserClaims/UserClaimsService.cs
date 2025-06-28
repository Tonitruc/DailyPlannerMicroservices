using BaseBuldingsBlocks.Middleware;

namespace UserService.UseCases.Services.UserClaims;

public class UserClaimsService(JwtClaimsMiddleware jwtClaimsMiddleware) : IUserClaimsService
{
    public string? GetUserEmail()
    {
        return jwtClaimsMiddleware.IsAuthorized && jwtClaimsMiddleware.IsValidToken
            ? jwtClaimsMiddleware.Email : null;
    }
}
