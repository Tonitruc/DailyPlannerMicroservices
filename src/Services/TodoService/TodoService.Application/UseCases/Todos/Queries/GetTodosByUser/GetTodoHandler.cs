using TodoService.Domain.Contracts.Repositories;
using TodoService.Application.Contracts.User;
using TodoService.Application.Dtos;
using AutoMapper;
using MediatR;

namespace TodoService.Application.UseCases.Todos.Queries.GetTodosByUser;

public record GetAllTodosByUserQuery : IRequest<IEnumerable<TodoDto>>;

public class GetAllTodosByUserQueryHandler(IRepositoryManager repository,
    IUserClaimsService userClaimsService,
    IMapper mapper)
    : IRequestHandler<GetAllTodosByUserQuery, IEnumerable<TodoDto>>
{
    public async Task<IEnumerable<TodoDto>> Handle(GetAllTodosByUserQuery request, CancellationToken cancellationToken)
    {
        var todos = await repository.Todos
            .GetAllTodosByUserAsync(userClaimsService.GetUserId(), cancellationToken);

        return mapper.Map<IEnumerable<TodoDto>>(todos);
    }
}
