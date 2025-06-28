using BaseBuldingsBlocks.Behaviors;
using BaseBuldingsBlocks.Middleware;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection;
using TodoTypeService.Data;
using TodoTypeService.Endpoints;

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

builder.Services.Configure<IOptions<RabbitMQSettings>>(cfg => 
    builder.Configuration.GetSection("RabbitMQ"));

builder.Services.AddMassTransit(x =>
{
    x.AddConsumers(typeof(Program).Assembly);

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQ:Host"], "/", h =>
        {
            h.Username(builder.Configuration["RabbitMQ:Username"]);
            h.Password(builder.Configuration["RabbitMQ:Password"]);
        });

        cfg.ConfigureEndpoints(context);
    });
});


#endregion

var app = builder.Build();

#region OtherUses

app.UseExceptionHandler(options => { });

#endregion

#region Endpoints

app.MapGet("/", () => "Todo type service is work...");
app.MapTodoTypes();

#endregion

#region DatabaseInitialization

await app.InitializeAsync();

#endregion

app.Run();

public class RabbitMQSettings
{
    public string Host { get; set; }
    public string Username { get; set; }    
    public string Password { get; set; }
}