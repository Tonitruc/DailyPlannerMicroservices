using FluentValidation;
using System.Globalization;
using TodoService.Application.Dtos;
using TodoService.Application.UseCases.Todos.Commands.CreateTodo;

namespace TodoService.Application.Validation.TodoValidators;

public class CreateTodoDtoValidator : AbstractValidator<CreateTodoCommand>
{
    private const string DateFormat = "dd.MM.yyyy HH:mm:ss";
    private const int MaxTitleLength = 100;
    private const int MaxDescriptionLength = 500;

    public CreateTodoDtoValidator()
    {
        RuleFor(todo => todo.TodoDto.Title)
            .NotNull()
            .NotEmpty()
            .MaximumLength(MaxTitleLength);

        RuleFor(todo => todo.TodoDto.Description)
            .MaximumLength(MaxDescriptionLength);

        RuleFor(todo => todo.TodoDto.StartDate)
            .NotEmpty()
            .Must(BeAValidDate).WithMessage("StartDate should be in format dd.MM.yyyy HH:mm:ss.")
            .Must(BeAFutureDate).WithMessage("StartDate не может быть в прошлом.");

        RuleFor(todo => todo.TodoDto.EndDate)
            .Must(BeAValidDateOrNull).WithMessage("EndDate должен быть в формате dd.MM.yyyy HH:mm:ss или пустым.");

        RuleFor(todo => todo.TodoDto)
            .Must(dto =>
            {
                if (!DateTime.TryParseExact(dto.StartDate, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var start))
                    return false;

                if (string.IsNullOrEmpty(dto.EndDate))
                    return true;

                if (!DateTime.TryParseExact(dto.EndDate, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var end))
                    return false;

                return start < end;
            })
            .WithMessage("StartDate должен быть меньше EndDate.");
    }

    private bool BeAValidDate(string? dateStr)
    {
        if (dateStr is null)
            return true;

        return DateTime.TryParseExact(dateStr, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
    }

    private bool BeAFutureDate(string? dateStr)
    {
        if (DateTime.TryParseExact(dateStr, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
        {
            return date >= DateTime.Now;
        }
        return false;
    }

    private bool BeAValidDateOrNull(string? dateStr)
    {
        if (string.IsNullOrEmpty(dateStr))
            return true;

        return DateTime.TryParseExact(dateStr, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
    }
}