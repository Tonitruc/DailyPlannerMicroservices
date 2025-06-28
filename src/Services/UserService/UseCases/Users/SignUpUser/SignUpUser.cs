using Microsoft.AspNetCore.Identity;
using FluentValidation.Results;
using UserService.Exceptions;
using UserService.Models;
using FluentValidation;
using UserService.Dtos;
using AutoMapper;
using MediatR;

namespace UserService.UseCases.Users.SignUpUser;

public record SignUpUserCommand(SignUpUserDto User) : IRequest<bool>;

public class SignInUserCommandHandler(UserManager<User> userManager,
    IMapper mapper) 
    : IRequestHandler<SignUpUserCommand, bool>
{
    public async Task<bool> Handle(SignUpUserCommand request, CancellationToken cancellationToken)
    {
        var existUser = await userManager.FindByEmailAsync(request.User.Email);

        if(existUser is not null)
            throw new UserAlreadyExist(request.User.Email);

        var newUser = mapper.Map<User>(request.User);
        var result = await userManager.CreateAsync(newUser, request.User.Password);

        if (!result.Succeeded)
            throw new ValidationException("User creation failed.", result.Errors
                .Select(error => new ValidationFailure { ErrorMessage = error.Description }));

        await userManager.AddToRoleAsync(newUser, "User");

        return result.Succeeded;
    }
}
