using UserService.Dtos;
using UserService.Models;

namespace UserService.UseCases.Services.Authentication;

public interface IAuthenticateManager
{
    Task<BaerarTokenDto> GenerateTokensAsync(User user);
}
