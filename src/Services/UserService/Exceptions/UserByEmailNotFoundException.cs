using BaseBuldingsBlocks.Exceptions.Base;

namespace UserService.Exceptions;

public class UserByEmailNotFoundException(string email) 
    : NotFoundException($"There is no user with current email: {email}.")
{ }
