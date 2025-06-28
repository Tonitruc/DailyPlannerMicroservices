using MediatR;
using UserService.Dtos;
using UserService.UseCases.Services.UserRead;

namespace UserService.UseCases.Users.GetUserByEmail;

public record GetUserByEmailQuery(string Email) : IRequest<BriefUserDto>;

public class GetUserByEmailQueryHandler(IUserReadService userReadService) 
    : IRequestHandler<GetUserByEmailQuery, BriefUserDto>
{
    public async Task<BriefUserDto> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        return await userReadService.GetBriefUserInformationAsync(request.Email, cancellationToken);
    }
}
