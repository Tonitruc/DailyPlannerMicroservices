using Microsoft.EntityFrameworkCore;
using BaseBuldingsBlocks.Middleware;
using BaseBuldingsBlocks.Behaviors;
using TodoTypeService.Endpoints;
using TodoTypeService.Data;
using System.Reflection;
using FluentValidation;
using MassTransit;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

#region Database 

builder.Services.AddScoped<ApplicationDatabaseInitializer>();
builder.Services.AddDbContext<ApplicationDbContext>((sp, options) => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresSQL"))
           .AddSeedingAsync(sp));

#endregion

#region MediatR

builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());

    options.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
});

#endregion

#region OtherServices

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

#endregion

#region Message Broker

builder.Services.AddMassTransit(cfg =>
{
    cfg.UsingRabbitMq((context, config) =>
    {
        var settings = builder.Configuration.GetSection("RabbitMQSettings");

        config.Host(settings["Host"], options =>
        {
            options.Username(settings["Username"]!);
            options.Password(settings["Password"]!);
        });
    });
});

#endregion

var app = builder.Build();

app.UseExceptionHandler(options => { });

#region Endpoints

app.MapGet("/", () => "Todo type service is work...");
app.MapTodoTypes();

#endregion

#region DatabaseInitialization

await app.InitializeAsync();

#endregion

app.Run();