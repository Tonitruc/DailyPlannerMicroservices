using TodoService.Application.Dtos;
using TodoService.Domain.Contracts.Repositories;
using TodoService.Domain.Models;
using AutoMapper;
using MediatR;

namespace TodoService.Application.UseCases.Todos.Commands.CreateTodo;

public record CreateTodoCommand(CreateTodoDto TodoDto) : IRequest<int>;

public class CreateTodoHandler(IRepositoryManager repository,
    IMapper mapper) 
    : IRequestHandler<CreateTodoCommand, int>
{
    public async Task<int> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
    {
        var newTodo = mapper.Map<Todo>(request.TodoDto);

        newTodo = await repository.Todos.CreateTodoAsync(newTodo, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return newTodo.Id;
    }
}
