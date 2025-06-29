using TodoService.Domain.Contracts.Repositories;
using TodoService.Domain.Events;
using MassTransit;

namespace TodoService.Application.UseCases.Todos.Events;

public class TodoStartEventHandler(IRepositoryManager repositoryManager) 
    : IConsumer<TodoStartEvent>
{
    public async Task Consume(ConsumeContext<TodoStartEvent> context)
    {
        context.Message.Todo.CheckStatus();
        repositoryManager.Todos.UpdateTodo(context.Message.Todo);
        await repositoryManager.SaveChangesAsync(CancellationToken.None);
    }
}
