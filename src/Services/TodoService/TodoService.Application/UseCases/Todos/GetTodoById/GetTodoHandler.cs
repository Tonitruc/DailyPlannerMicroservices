using TodoService.Application.Common.Exceptions.Specific;
using TodoService.Application.Dtos;
using TodoService.Domain.Contracts.Repositories;
using AutoMapper;
using MediatR;

namespace TodoService.Application.UseCases.Todos.GetTodoById;

public record GetTodoByIdQuery(int Id) : IRequest<TodoDto>;

public class GetTodoQueryHandler(IRepositoryManager repository,
    IMapper mapper)
    : IRequestHandler<GetTodoByIdQuery, TodoDto>
{
    public async Task<TodoDto> Handle(GetTodoByIdQuery request, CancellationToken cancellationToken)
    {
        var todo = await repository.Todos
            .GetTodoByIdAsync(request.Id, cancellationToken)
            ?? throw new TodoByIdNotFoundException(request.Id);

        return mapper.Map<TodoDto>(todo);
    }
}
