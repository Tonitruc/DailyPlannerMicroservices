using TodoService.Application.Contracts.User;
using TodoService.Presentation.Extensions;
using TodoService.Presentation.Services;
using TodoService.Infrastracture.Data;
using System.Text.Json.Serialization;
using BaseBuldingsBlocks.Middleware;
using TodoService.Infrastracture;
using TodoService.Application;

var builder = WebApplication.CreateBuilder(args);

builder.AddApplicationServices();
builder.AddInfrastractureServices();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.AppendTrailingSlash = true;
});

builder.Services.AddScoped<JwtClaimsMiddleware>();
builder.Services.AddScoped<IUserClaimsService, UserClaimsService>();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();

app.UseExceptionHandler(options => { });

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<JwtClaimsMiddleware>();

app.MapGet("/", () => "Todo service is work!...");
app.MapEndpoints(); //TODO Controllers or Curter library?

await app.InitializeDatabaseAsync();

app.Run();
