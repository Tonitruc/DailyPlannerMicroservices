using AutoMapper;
using TodoTypeService.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace TodoTypeService.TodoTypes.TodoTypeExist;

public sealed record TodoTypeExistsQuery(string Name) : IRequest<bool>;  

public class TodoTypeExistQueryHandler : IRequestHandler<TodoTypeExistsQuery, bool>
{
    private readonly ApplicationDbContext _context;


    public TodoTypeExistQueryHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
    }


    public async Task<bool> Handle(TodoTypeExistsQuery request, CancellationToken ct)
    {
        return await _context.TodoTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(ct => ct.Name == request.Name, ct)
            is not null;
    }
}
