using UserService.Dtos;
using MediatR;
using UserService.UseCases.Services.UserRead;

namespace UserService.UseCases.Users.GetAllUsers;

public record GetAllUsersQuery : IRequest<IEnumerable<BriefUserDto>>;

public class GetAllUserQueryHandler(IUserReadService userReadService) 
    : IRequestHandler<GetAllUsersQuery, IEnumerable<BriefUserDto>>
{
    public async Task<IEnumerable<BriefUserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        return await userReadService.GetUsersInformationAsync(cancellationToken);
    }
}
