using BaseBuldingsBlocks.Middleware;
using TodoService.Application.Contracts.User;

namespace TodoService.Presentation.Services;

public class UserClaimsService(JwtClaimsMiddleware jwtClaimsMiddleware) 
    : IUserClaimsService
{
    public string? GetUserId()
    {
        return jwtClaimsMiddleware.IsAuthorized && jwtClaimsMiddleware.IsValidToken
            ? jwtClaimsMiddleware.UserId : null;
    }

    public string? GetUserEmail()
    {
        return jwtClaimsMiddleware.IsAuthorized && jwtClaimsMiddleware.IsValidToken
            ? jwtClaimsMiddleware.Email : null;
    }
}
