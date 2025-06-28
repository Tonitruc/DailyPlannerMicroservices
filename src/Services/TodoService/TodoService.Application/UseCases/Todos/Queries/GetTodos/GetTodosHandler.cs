using TodoService.Application.Dtos;
using TodoService.Domain.Contracts.Repositories;
using AutoMapper;
using MediatR;

namespace TodoService.Application.UseCases.Todos.Queries.GetTodos;

public sealed record GetTodosQuery() : IRequest<IEnumerable<TodoDto>>;

public class GetTodosHandler(IRepositoryManager repositoryManager,
    IMapper mapper) 
    : IRequestHandler<GetTodosQuery, IEnumerable<TodoDto>>
{
    public async Task<IEnumerable<TodoDto>> Handle(GetTodosQuery request, CancellationToken cancellationToken)
    {
        var todos = await repositoryManager.Todos.GetAllTodosAsync(cancellationToken);

        return mapper.Map<IEnumerable<TodoDto>>(todos);
    }
}
