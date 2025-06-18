namespace TodoTypeService.Exceptions;

public class TodoTypeAlreadyExistException : Exception
{
    public TodoTypeAlreadyExistException(string name)
        : base($"Todo type with name {name} already exist.") { }
}
