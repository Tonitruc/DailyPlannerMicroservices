using UserService.Dtos;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UserService.Models;
using UserService.UseCases.Services.Authentication;
using Microsoft.EntityFrameworkCore;

namespace UserService.UseCases.Authentication.RefreshToken;

public record RefreshTokenCommand(string RefreshToken) : IRequest<BaerarTokenDto>;

public class RefreshTokenCommandHandler(UserManager<User> userManager,
    IAuthenticateManager authenticateManager)
    : IRequestHandler<RefreshTokenCommand, BaerarTokenDto>
{
    public async Task<BaerarTokenDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.Users
            .FirstAsync(user => user.RefreshToken == request.RefreshToken, cancellationToken: cancellationToken);

        var token = await authenticateManager.GenerateTokensAsync(user);

        user.RefreshToken = token.RefreshToken;
        user.ExpireRefreshToken = token.ExpireRefreshToken;

        await userManager.UpdateAsync(user);

        return token;
    }
}
