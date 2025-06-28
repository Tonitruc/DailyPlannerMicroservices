using FluentValidation;
using TodoService.Application.Dtos;
using TodoService.Application.UseCases.Todos.Commands.CreateTodo;

namespace TodoService.Application.Validation.TodoValidators;

public class CreateTodoDtoValidator : AbstractValidator<CreateTodoCommand>
{
    //TODO add messages
    public CreateTodoDtoValidator()
    {
        RuleFor(todo => todo.TodoDto.Title)
            .NotNull()
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(todo => todo.TodoDto.Description)
            .MaximumLength(500);

        RuleFor(todo => todo.TodoDto.StartDate)
            .LessThan(todo => todo.TodoDto.EndDate)
            .GreaterThanOrEqualTo(_ => DateTime.Now);
    }
}
