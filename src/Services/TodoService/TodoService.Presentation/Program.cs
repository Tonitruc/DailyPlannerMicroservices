using BaseBuldingsBlocks.Middleware;
using TodoService.Application;
using TodoService.Infrastracture;
using TodoService.Infrastracture.Data;
using TodoService.Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddApplicationServices();
builder.AddInfrastractureServices();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.AppendTrailingSlash = true;
});

var app = builder.Build();

app.UseExceptionHandler(options => { });

app.MapGet("/", () => "Todo service is work!...");
app.MapEndpoints(); //TODO Controllers or Curter library?

await app.InitializeDatabaseAsync();

app.Run();
