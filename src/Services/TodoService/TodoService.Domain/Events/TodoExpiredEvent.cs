using TodoService.Domain.Models;

namespace TodoService.Domain.Events;

public class TodoExpiredEvent(Todo todo)
{
    public Todo Todo { get; set; } = todo;
}
