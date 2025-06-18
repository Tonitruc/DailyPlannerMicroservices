namespace TodoTypeService.Exceptions;

public class TodoTypeNoFoundException : Exception
{
    public TodoTypeNoFoundException()
        : base($"There is no such Todo type.") { }

    public TodoTypeNoFoundException(string message)
        : base(message) { }
}
