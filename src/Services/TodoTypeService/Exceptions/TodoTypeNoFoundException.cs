using BaseBuldingsBlocks.Exceptions.Base;

namespace TodoTypeService.Exceptions;

public class TodoTypeNoFoundException : NotFoundException
{
    public TodoTypeNoFoundException()
        : base($"There is no such Todo type.") { }

    public TodoTypeNoFoundException(string message)
        : base(message) { }
}
