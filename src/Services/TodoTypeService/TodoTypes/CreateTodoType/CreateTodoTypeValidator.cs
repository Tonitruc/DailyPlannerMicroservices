using FluentValidation;

namespace TodoTypeService.TodoTypes.CreateTodoType;

public class CreateTodoTypeValidator : AbstractValidator<CreateTodoTypeCommand>
{
    public const int MaximumNameLength = 50;


    public CreateTodoTypeValidator()
    {
        RuleFor(o => o.TodoType.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(MaximumNameLength);
    }
}
