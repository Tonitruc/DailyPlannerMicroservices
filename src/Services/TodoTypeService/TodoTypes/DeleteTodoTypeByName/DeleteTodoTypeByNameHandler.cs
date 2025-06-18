using TodoTypeService.Data;
using TodoTypeService.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace TodoTypeService.TodoTypes.DeleteTodoTypeByName;

public sealed record DeleteTodoTypeByNameCommand(string Name) : IRequest<bool>;

public class DeleteTodoTypeByNameHandler : IRequestHandler<DeleteTodoTypeByNameCommand, bool>
{
    private readonly ApplicationDbContext _context;


    public DeleteTodoTypeByNameHandler(ApplicationDbContext context)
    {
        _context = context;
    }


    public async Task<bool> Handle(DeleteTodoTypeByNameCommand request, CancellationToken ct)
    {
        var TodoType = await _context.TodoTypes
            .FirstOrDefaultAsync(ct => ct.Name == request.Name, ct)
            ?? throw new TodoTypeNoFoundException();

        _context.Remove(TodoType);
        await _context.SaveChangesAsync(ct);

        return true;
    }
}
