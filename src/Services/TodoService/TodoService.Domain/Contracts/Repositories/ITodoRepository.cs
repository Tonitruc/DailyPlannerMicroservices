using TodoService.Domain.Models;

namespace TodoService.Domain.Contracts.Repositories;

public interface ITodoRepository
{
    Task<IEnumerable<Todo>> GetAllTodosAsync(CancellationToken cancellationToken);
    Task<IEnumerable<Todo>> GetAllTodosByUserAsync(string userId, CancellationToken cancellationToken);
    Task<Todo?> GetTodoByIdAsync(int id, CancellationToken cancellationToken);
    Task<Todo> CreateTodoAsync(Todo todo, CancellationToken cancellationToken);
    Todo CompleteTodo(Todo todo);
    Todo UpdateTodo(Todo todo);
    bool DeleteTodo(Todo todo);
}
