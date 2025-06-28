using Microsoft.AspNetCore.Identity;

namespace UserService.Models;

public class User : IdentityUser
{
    public string? RefreshToken { get; set; } 
    public DateTime ExpireRefreshToken { get; set; }
}
