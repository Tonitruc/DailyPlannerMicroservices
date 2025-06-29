using TodoService.Application.UseCases.Todos.Commands.DeleteTodoById;
using TodoService.Application.UseCases.Todos.Queries.GetTodosByUser;
using TodoService.Application.UseCases.Todos.Queries.GetTodoById;
using TodoService.Application.UseCases.Todos.Commands.CreateTodo;
using TodoService.Application.UseCases.Todos.Queries.GetTodos;
using Microsoft.AspNetCore.Http.HttpResults;
using TodoService.Presentation.Extensions;
using TodoService.Domain.Constants;
using TodoService.Application.Dtos;
using MediatR;

namespace TodoService.Presentation.Endpoints;

public class Todos : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetAllTodos, "all").RequireAuthorization(Policies.AdminOnly);

        app.MapGroup(this)
            .MapGet(GetAllTodosByUser).RequireAuthorization();

        app.MapGroup(this)
            .MapGet(GetTodoById, "{id:int}").RequireAuthorization(Policies.AdminOnly);

        app.MapGroup(this)
            .MapPost(CreateTodo).RequireAuthorization();

        app.MapGroup(this)
            .MapDelete(DeleteTodoById, "{id:int}").RequireAuthorization();
    }

    public async Task<Ok<IEnumerable<TodoDto>>> GetAllTodos(ISender sender)
    {
        var result = await sender.Send(new GetTodosQuery());

        return TypedResults.Ok(result);
    }

    public async Task<Ok<IEnumerable<TodoDto>>> GetAllTodosByUser(ISender sender)
    {
        var result = await sender.Send(new GetAllTodosByUserQuery());

        return TypedResults.Ok(result);
    }

    public async Task<Ok<TodoDto>> GetTodoById(int id, ISender sender)
    {
        var result = await sender.Send(new GetTodoByIdQuery(id));

        return TypedResults.Ok(result);
    }

    public async Task<Created> CreateTodo(CreateTodoDto todoDto, ISender sender)
    {
        var result = await sender.Send(new CreateTodoCommand(todoDto));

        return TypedResults.Created($"{nameof(Todos)}/{result}");
    }

    public async Task<Ok<bool>> DeleteTodoById(int id, ISender sender)
    {
        var result = await sender.Send(new DeleteTodoCommand(id));

        return TypedResults.Ok(result);
    }
}
