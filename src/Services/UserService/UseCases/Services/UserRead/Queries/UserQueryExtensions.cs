using Microsoft.AspNetCore.Identity;
using UserService.Dtos;
using UserService.Models;

namespace UserService.UseCases.Services.UserRead.Queries;

public static class UserQueryExtensions
{
    public static IQueryable<BriefUserDto> BriefInfoWithRole(this IQueryable<User> query, 
        IEnumerable<IdentityUserRole<string>> userRoles, 
        IEnumerable<IdentityRole> roles)
    {
        return query
            .Join(userRoles,
                u => u.Id,
                ur => ur.UserId,
                (user, role) => new { user.Email, user.UserName, role.RoleId })
            .Join(roles,
                u => u.RoleId,
                r => r.Id,
                (user, role) => new BriefUserDto { Email = user.Email, UserName = user.UserName, Role = role.Name });
    }
}
