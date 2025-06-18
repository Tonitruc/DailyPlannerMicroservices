using FluentValidation;

namespace TodoTypeService.TodoTypes.CreateTodoType;

public class CreateTodoTypeValidator : AbstractValidator<CreateTodoTypeDto>
{
    public const int MaximumNameLength = 50;


    public CreateTodoTypeValidator()
    {
        RuleFor(o => o.Name)
            .NotNull()
            .NotEmpty()
            .MaximumLength(MaximumNameLength);
    }
}
