using TodoService.Domain.Contracts.Repositories;
using Microsoft.Extensions.Logging;
using TodoService.Domain.Events;
using MassTransit;

namespace TodoService.Application.UseCases.Todos.Events;

public class TodoExpiredEventHandler(ILogger<TodoExpiredEventHandler> logger,
    IRepositoryManager repositoryManager) 
    : IConsumer<TodoExpiredEvent>
{
    public async Task Consume(ConsumeContext<TodoExpiredEvent> context)
    {
        context.Message.Todo.CheckStatus();
        repositoryManager.Todos.UpdateTodo(context.Message.Todo);
        await repositoryManager.SaveChangesAsync(CancellationToken.None); //TODO WTF
    }
}
