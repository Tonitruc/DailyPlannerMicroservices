using AutoMapper;
using AutoMapper.QueryableExtensions;
using TodoTypeService.TodoTypes.Commons;
using TodoTypeService.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace TodoTypeService.TodoTypes.GetTodoTypes;

public record GetTodoTypesQuery() : IRequest<IEnumerable<TodoTypeBriefDto>>;


public class GetTodoTypesQueryHandler : IRequestHandler<GetTodoTypesQuery, IEnumerable<TodoTypeBriefDto>>
{
    private readonly ApplicationDbContext _context;

    private readonly IMapper _mapper;


    public GetTodoTypesQueryHandler(ApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    public async Task<IEnumerable<TodoTypeBriefDto>> Handle(GetTodoTypesQuery request, CancellationToken ct)
    {
        return await _context.TodoTypes
            .AsNoTracking()
            .ProjectTo<TodoTypeBriefDto>(_mapper.ConfigurationProvider)
            .ToListAsync(ct);
    }
}