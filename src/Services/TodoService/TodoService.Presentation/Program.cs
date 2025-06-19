using TodoService.Application.UseCases.Todos.GetTodos;
using TodoService.Application;
using TodoService.Infrastracture;
using TodoService.Infrastracture.Data;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.AddApplicationServices();
builder.AddInfrastractureServices();

var app = builder.Build();

app.MapGet("/", () => "Todo service is work!...");

app.MapGet("/api/todo", async (ISender sender) =>
{
    var result = await sender.Send(new GetTodosQuery());

    return result;
});

await app.InitializeDatabaseAsync();

app.Run();
