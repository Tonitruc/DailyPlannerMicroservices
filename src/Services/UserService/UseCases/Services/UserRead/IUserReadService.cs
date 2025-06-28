using UserService.Dtos;

namespace UserService.UseCases.Services.UserRead;

public interface IUserReadService
{
    Task<IEnumerable<BriefUserDto>> GetUsersInformationAsync(CancellationToken cancellationToken);
    Task<BriefUserDto> GetBriefUserInformationAsync(string email, CancellationToken cancellationToken);
}
