using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Dtos;
using UserService.Exceptions;
using UserService.Models;
using UserService.UseCases.Services.UserRead.Queries;

namespace UserService.UseCases.Services.UserRead;

public class UserReadService(ApplicationDbContext context,
    UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager) : IUserReadService
{
    public async Task<IEnumerable<BriefUserDto>> GetUsersInformationAsync(CancellationToken cancellationToken = default)
    {
        return await context.Users
            .BriefInfoWithRole(context.UserRoles, context.Roles)
            .ToListAsync(cancellationToken);
    }

    public async Task<BriefUserDto> GetBriefUserInformationAsync(string email, CancellationToken cancellationToken = default)
    {
        return await context.Users
            .Where(u  => u.Email == email)
            .BriefInfoWithRole(context.UserRoles, context.Roles)
            .SingleOrDefaultAsync(cancellationToken)
            ?? throw new UserByEmailNotFoundException(email);
    }
}
