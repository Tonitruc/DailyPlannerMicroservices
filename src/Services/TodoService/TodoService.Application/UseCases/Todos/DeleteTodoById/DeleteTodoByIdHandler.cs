using MediatR;
using TodoService.Application.Common.Exceptions.Specific;
using TodoService.Domain.Contracts.Repositories;

namespace TodoService.Application.UseCases.Todos.DeleteTodoById;

public record DeleteTodoCommand(int Id) : IRequest<bool>;

public class DeleteTodoByIdHandler(IRepositoryManager repository) : IRequestHandler<DeleteTodoCommand, bool>
{
    public async Task<bool> Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
    {
        var existTodo = await repository.Todos
            .GetTodoByIdAsync(request.Id, cancellationToken)
            ?? throw new TodoByIdNotFoundException(request.Id);

        var result = repository.Todos
            .DeleteTodo(existTodo);

        await repository.SaveChangesAsync(cancellationToken);

        return result;
    }
}