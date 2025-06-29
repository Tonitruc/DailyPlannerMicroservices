using TodoService.Domain.Models;

namespace TodoService.Domain.Extensions;

public static class QueryableTodoExtensions
{
    public static IQueryable<Todo> FindByUser(this IQueryable<Todo> query, string userId)
    {
        return query.Where(t => t.UserExternalId == userId);
    }
}
