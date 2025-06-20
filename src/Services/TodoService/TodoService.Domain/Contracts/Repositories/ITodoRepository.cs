using TodoService.Domain.Models;

namespace TodoService.Domain.Contracts.Repositories;

public interface ITodoRepository
{
    Task<IEnumerable<Todo>> GetAllTodosAsync(CancellationToken cancellationToken);
    Task<Todo?> GetTodoByIdAsync(int id, CancellationToken cancellationToken);
    Task<Todo> CreateTodoAsync(Todo todo, CancellationToken cancellationToken);
    bool DeleteTodo(Todo todo);
}
