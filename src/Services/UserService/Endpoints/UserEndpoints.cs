using UserService.UseCases.Users.DeleteUserByEmail;
using UserService.UseCases.Users.GetUserByEmail;
using UserService.UseCases.Users.GetAllUsers;
using UserService.UseCases.Users.SignUpUser;
using Microsoft.AspNetCore.Authorization;
using MediatR;

namespace UserService.Endpoints;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUserEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/users");

        group.MapGet("/", [Authorize] async (ISender sender) =>
        {
            var result = await sender.Send(new GetAllUsersQuery());

            return Results.Ok(result);
        });

        group.MapGet("/{email}", [Authorize] async (string email, ISender sender) =>
        {
            var result = await sender.Send(new GetUserByEmailQuery(email));
            return Results.Ok(result);
        })
        .WithName("GetUserByEmail");

        group.MapPost("/", async (SignUpUserCommand request, ISender sender) =>
        {
            var result = await sender.Send(request);

            return Results.CreatedAtRoute("GetUserByEmail", new {request.User.Email}, result);
        });

        group.MapDelete("/{email}", [Authorize(Roles = "Admin")] async (string email, ISender sender) =>
        {
            var result = await sender.Send(new DeleteUserByEmailCommand(email));

            return Results.Ok(result);
        });

        return group;
    }
}
