using TodoService.Domain.Contracts.Repositories;
using TodoService.Domain.Extensions;
using Microsoft.EntityFrameworkCore;
using TodoService.Domain.Models;
using TodoService.Domain.Enums;

namespace TodoService.Infrastracture.Data.Repositories;

public class TodoRepository(ApplicationDbContext context) : ITodoRepository
{
    public async Task<IEnumerable<Todo>> GetAllTodosAsync(CancellationToken cancellationToken = default)
    {
        return await context.Todos
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Todo>> GetAllTodosByUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await context.Todos
            .AsNoTracking()
            .FindByUser(userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Todo?> GetTodoByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await context.Todos
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Todo> CreateTodoAsync(Todo todo, CancellationToken cancellationToken = default)
    {
        var entry = await context.Todos
            .AddAsync(todo, cancellationToken);

        return entry.Entity;
    }

    public Todo CompleteTodo(Todo todo)
    {
        todo.Status = TodoStatuses.Completed;

        return UpdateTodo(todo);
    }

    public Todo UpdateTodo(Todo todo)
    {
        var entry = context.Todos
            .Update(todo);

        return entry.Entity;
    }

    public bool DeleteTodo(Todo todo)
    {
        context.Remove(todo);
        return true;
    }
}
