using TodoService.Domain.Abstractions;
using TodoService.Domain.Enums;

namespace TodoService.Domain.Models;

public class Todo : Entity<int>
{
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public DateTime StartDate {  get; set; }

    public DateTime? EndDate { get; set; }

    //public TimeSpan? ExecutionTime { get; set; }

    //public TodoStatus Status { get; set; }
}
