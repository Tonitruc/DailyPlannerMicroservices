using TodoTypeService.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoTypeService.Exceptions;
using AutoMapper;
using TodoTypeService.Models;

namespace TodoTypeService.TodoTypes.CreateTodoType;

public sealed record CreateTodoTypeCommand(CreateTodoTypeDto TodoType) : IRequest<bool>; 

public class CreateTodoTypeCommandHandler : IRequestHandler<CreateTodoTypeCommand, bool>
{
    private readonly ApplicationDbContext _context;

    private readonly IMapper _mapper;


    public CreateTodoTypeCommandHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    public async Task<bool> Handle(CreateTodoTypeCommand request, CancellationToken ct)
    {
        var TodoType = await _context.TodoTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(ct => ct.Name == request.TodoType.Name, ct);

        if (TodoType is not null)
            throw new TodoTypeAlreadyExistException(request.TodoType.Name);

        await _context.TodoTypes.AddAsync(_mapper.Map<TodoType>(request.TodoType), ct);
        await _context.SaveChangesAsync(ct);

        return true;
    }
}
