using TodoService.Application.Contracts.MessageBroker;
using TodoService.Domain.Contracts.Repositories;
using TodoService.Application.Contracts.User;
using TodoService.Application.Dtos;
using TodoService.Domain.Events;
using TodoService.Domain.Models;
using AutoMapper;
using MediatR;

namespace TodoService.Application.UseCases.Todos.Commands.CreateTodo;

public record CreateTodoCommand(CreateTodoDto TodoDto) : IRequest<int>;

public class CreateTodoHandler(IRepositoryManager repository,
    IMapper mapper,
    IUserClaimsService userClaimsService,
    IDelayMessagePublisher publisher) 
    : IRequestHandler<CreateTodoCommand, int>
{
    public async Task<int> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
    {
        var newTodo = mapper.Map<Todo>(request.TodoDto);
        newTodo.UserExternalId = userClaimsService.GetUserId(); //TODO null

        newTodo = await repository.Todos.CreateTodoAsync(newTodo, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        if(!newTodo.IsStarted())
            await publisher.PublishAsync(new TodoStartEvent(newTodo), newTodo.GetTimeBeforeStart(), cancellationToken);

        if (newTodo.IsLimited())
            await publisher.PublishAsync(new TodoExpiredEvent(newTodo), newTodo.GetLeadTime(), cancellationToken);

        return newTodo.Id;
    }
}
