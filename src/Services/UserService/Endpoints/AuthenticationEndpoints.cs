using UserService.UseCases.Authentication.RefreshToken;
using UserService.UseCases.Authentication.SignOut;
using UserService.UseCases.Authentication.SignIn;
using Microsoft.AspNetCore.Authorization;
using MediatR;

namespace UserService.Endpoints;

public static class AuthenticationEndpoints
{
    public static RouteGroupBuilder MapAuthenticationEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api");

        group.MapPost("signin", async (SignInCommand request, ISender sender) =>
        {
            var result = await sender.Send(request);

            return Results.Ok(result);
        });

        group.MapPost("refresh", async (RefreshTokenCommand request, ISender sender) =>
        {
            var result = await sender.Send(request);

            return Results.Ok(result);
        });

        group.MapPost("signOut", [Authorize] async (ISender sender) =>
        {
            var result = await sender.Send(new SignOutCommand());

            return Results.Ok(result);
        });

        return group;
    }
}
