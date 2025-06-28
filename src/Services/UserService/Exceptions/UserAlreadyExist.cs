namespace UserService.Exceptions;

public class UserAlreadyExist(string email)
    : Exception($"User with current email {email} already exist")
{ }
