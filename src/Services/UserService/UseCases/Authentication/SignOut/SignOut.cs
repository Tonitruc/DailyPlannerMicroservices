using UserService.UseCases.Services.UserClaims;
using Microsoft.AspNetCore.Identity;
using UserService.Exceptions;
using UserService.Models;
using MediatR;

namespace UserService.UseCases.Authentication.SignOut;

public record SignOutCommand : IRequest<bool>;

public class SignOutCommandHandler(IUserClaimsService userService,
    UserManager<User> userManager) 
    : IRequestHandler<SignOutCommand, bool>
{
    public async Task<bool> Handle(SignOutCommand request, CancellationToken cancellationToken)
    {
        var email = userService.GetUserEmail();

        if (string.IsNullOrEmpty(email))
            return false;

        var user = await userManager.FindByEmailAsync(email)
            ?? throw new UserByEmailNotFoundException(email);
        
        user.RefreshToken = null;

        await userManager.UpdateAsync(user);
        return true;
    }
}
