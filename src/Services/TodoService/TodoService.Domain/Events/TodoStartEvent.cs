using TodoService.Domain.Models;

namespace TodoService.Domain.Events;

public class TodoStartEvent(Todo todo)
{
    public Todo Todo { get; set; } = todo;
}
