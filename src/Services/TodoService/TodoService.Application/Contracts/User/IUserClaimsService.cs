namespace TodoService.Application.Contracts.User;

public interface IUserClaimsService
{
    string? GetUserId();
    string? GetUserEmail();
}
