namespace TodoService.Domain.Contracts.User;

public interface IUserClaimsService
{
    string? GetUserId();
    string? GetUserEmail();
}
