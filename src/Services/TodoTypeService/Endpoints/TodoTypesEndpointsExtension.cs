using TodoTypeService.TodoTypes.GetTodoTypes;
using TodoTypeService.TodoTypes.GetTodoTypeById;
using TodoTypeService.TodoTypes.CreateTodoType;
using TodoTypeService.TodoTypes.TodoTypeExist;
using TodoTypeService.TodoTypes.DeleteTodoTypeByName;
using MediatR;

namespace TodoTypeService.Endpoints;

public static class TodoTypesEndpointsExtension
{
    public static RouteGroupBuilder MapTodoTypes(this WebApplication app)
    {
        var group = app.MapGroup("/api/todo-types");

        group.MapGet("/", async (ISender sender) =>
        {
            var result = await sender.Send(new GetTodoTypesQuery());

            return Results.Ok(result);
        });

        group.MapGet("/{id:int}", async (int id, ISender sender) =>
        {
            var result = await sender.Send(new GetTodoTypeByIdQeury(id));

            return Results.Ok(result);
        });

        group.MapPost("/", async (CreateTodoTypeCommand request, ISender sender) =>
        {
            var result = await sender.Send(request);

            return Results.Created();
        });

        group.MapGet("/exists/{name}", async (string name, ISender sender) =>
        {
            var result = await sender.Send(new TodoTypeExistsQuery(name));

            return Results.Ok(result);
        });

        group.MapPost("/exists", async (TodoTypesExistsQuery request, ISender sender) =>
        {
            var result = await sender.Send(request);

            return Results.Ok(result);
        });

        group.MapDelete("/{name}", async (string name, ISender sender) =>
        {
            var result = await sender.Send(new DeleteTodoTypeByNameCommand(name));

            return Results.Ok(result);
        });

        return group;
    }
}