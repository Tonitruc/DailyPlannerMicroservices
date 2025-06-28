using FluentValidation;
using UserService.UseCases.Users.SignUpUser;

namespace UserService.Validation;

public class SignUpUserValidation : AbstractValidator<SignUpUserCommand>
{
    private const int MaxUserNameLength = 50;
    private const int MinUserNameLength = 3;


    public SignUpUserValidation()
    {
        RuleFor(o => o.User.UserName)
            .NotEmpty()
            .MinimumLength(MinUserNameLength)
            .MaximumLength(MaxUserNameLength);
            //.EmailAddress();
    }
}
