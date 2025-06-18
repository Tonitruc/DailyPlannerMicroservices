using TodoTypeService.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace TodoTypeService.TodoTypes.TodoTypeExist;

public sealed record TodoTypesExistsQuery(IEnumerable<string> TodoTypes) : IRequest<TodoTypeExistsResult>;  
public sealed record TodoTypeExistsResult(bool Exists, IEnumerable<string> TodoTypesNotExists);

public class TodoTypesExistsQueryHandler : IRequestHandler<TodoTypesExistsQuery, TodoTypeExistsResult>
{
    private readonly ApplicationDbContext _context;


    public TodoTypesExistsQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }


    public async Task<TodoTypeExistsResult> Handle(TodoTypesExistsQuery request, CancellationToken ct)
    {
        var existsTodoTypes = await _context.TodoTypes
            .Where(ct => request.TodoTypes.Contains(ct.Name))
            .Select(item => item.Name)
            .ToListAsync();

        var notExistsTodoTypes = request.TodoTypes.Except(existsTodoTypes);

        return new TodoTypeExistsResult(!notExistsTodoTypes.Any(), notExistsTodoTypes);
    }
}
