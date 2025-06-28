using Microsoft.AspNetCore.Identity;
using UserService.Exceptions;
using UserService.Models;
using MediatR;

namespace UserService.UseCases.Users.DeleteUserByEmail;

public record DeleteUserByEmailCommand(string Email) : IRequest<bool>;

public class DeleteUserByEmailHandler(UserManager<User> userManager) 
    : IRequestHandler<DeleteUserByEmailCommand, bool>
{
    public async Task<bool> Handle(DeleteUserByEmailCommand request, CancellationToken cancellationToken)
    {
        var existUser = await userManager.FindByEmailAsync(request.Email)
            ?? throw new UserByEmailNotFoundException(request.Email);

        var result = await userManager.DeleteAsync(existUser);

        if (!result.Succeeded)
            throw new Exception("User deletion failed.");

        return result.Succeeded;
    }
}
