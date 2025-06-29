using TodoService.Domain.Abstractions;
using TodoService.Domain.Enums;

namespace TodoService.Domain.Models;

public class Todo : Entity<int>
{

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    private DateTime _startDate;
    public DateTime StartDate
    {
        get => _startDate;
        set 
        {
            _startDate = value.Kind == DateTimeKind.Unspecified
                ? DateTime.SpecifyKind(value, DateTimeKind.Utc)
                : value.ToUniversalTime();

            CheckStatus();
        } 
    }

    private DateTime? _endDate;
    public DateTime? EndDate
    {
        get => _endDate;
        set => _endDate = value.HasValue
            ? (value.Value.Kind == DateTimeKind.Unspecified
                ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc)
                : value.Value.ToUniversalTime())
            : null;
    }

    public int TypeExternalId { get; set; }

    public string UserExternalId { get; set; } = string.Empty;

    public TodoStatuses Status { get; set; } = TodoStatuses.NotStarted;


    public bool IsStarted() 
        => Status == TodoStatuses.InProgress;

    public bool IsLimited()
        => EndDate is not null;

    public TimeSpan GetLeadTime()
    {
        if(EndDate is null)
            return TimeSpan.Zero;

        return (EndDate - StartDate).Value;
    }

    public TimeSpan GetTimeBeforeStart()
    {
        if(IsStarted())
            return TimeSpan.Zero;

        return StartDate - DateTime.Now;
    }

    public void CheckStatus()
    {
        if (EndDate >= DateTime.Now)
            Status = TodoStatuses.Expired;
        else if(StartDate > DateTime.Now)
            Status = TodoStatuses.NotStarted;
    }

    public void MarkAsCompleted() => Status = TodoStatuses.Completed;
    public void StartTodo() => Status = TodoStatuses.InProgress;
}
