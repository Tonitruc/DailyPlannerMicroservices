using AutoMapper.QueryableExtensions;
using TodoTypeService.TodoTypes.Commons;
using TodoTypeService.Data;
using TodoTypeService.Exceptions;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using MediatR;

namespace TodoTypeService.TodoTypes.GetTodoTypeById;

public record GetTodoTypeByIdQeury(int Id) : IRequest<TodoTypeBriefDto>;

public class GetTodoTypeByIdQueryHandler : IRequestHandler<GetTodoTypeByIdQeury, TodoTypeBriefDto>
{
    private readonly ApplicationDbContext _context;

    private readonly IMapper _mapper;


    public GetTodoTypeByIdQueryHandler(ApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    public async Task<TodoTypeBriefDto> Handle(GetTodoTypeByIdQeury request, CancellationToken ct)
    {
        return await _context.TodoTypes
            .AsNoTracking()
            .Where(ct => ct.Id == request.Id)
            .ProjectTo<TodoTypeBriefDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(ct) ?? throw new TodoTypeNoFoundException($"There is not exists Todo type with id {request.Id}");
    }
}