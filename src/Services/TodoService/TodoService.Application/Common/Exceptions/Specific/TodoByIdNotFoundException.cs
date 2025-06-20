using BaseBuldingsBlocks.Exceptions.Base;

namespace TodoService.Application.Common.Exceptions.Specific;

public class TodoByIdNotFoundException(int Id) 
    : NotFoundException($"There is no todo with id: {Id}") { }
