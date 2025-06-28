using UserService.UseCases.Services.Authentication;
using Microsoft.AspNetCore.Identity;
using UserService.Exceptions;
using UserService.Models;
using UserService.Dtos;
using MediatR;

namespace UserService.UseCases.Authentication.SignIn;

public record SignInCommand(SignInDto User) : IRequest<BaerarTokenDto>;

public class SignInCommandHandler(IAuthenticateManager authenticationService,
    UserManager<User> userManager)
    : IRequestHandler<SignInCommand, BaerarTokenDto>
{
    public async Task<BaerarTokenDto> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var existUser = await userManager.FindByEmailAsync(request.User.Email)
            ?? throw new UserByEmailNotFoundException(request.User.Email);

        var checkPasswordResult = await userManager.CheckPasswordAsync(existUser, request.User.Password);

        if (!checkPasswordResult)
            throw new Exception(); //TODO SignIn error

        var tokens = await authenticationService.GenerateTokensAsync(existUser);

        existUser.RefreshToken = tokens.RefreshToken;
        existUser.ExpireRefreshToken = tokens.ExpireRefreshToken;

        await userManager.UpdateAsync(existUser);

        return tokens;
    }
}
