using FluentValidation;
using TodoService.Application.Dtos;

namespace TodoService.Application.Validations;

public class TodoDtoValidator : AbstractValidator<TodoDto>
{
    //TODO add messages
    public TodoDtoValidator()
    {
        RuleFor(todo => todo.Title)
            .NotNull()
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(todo => todo.Description)
            .MaximumLength(500);

        RuleFor(todo => todo.StartDate)
            .LessThan(todo => todo.EndDate)
            .GreaterThanOrEqualTo(_ => DateTime.Now);
    }
}
