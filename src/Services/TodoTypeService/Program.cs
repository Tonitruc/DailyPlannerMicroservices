using BaseBuldingsBlocks.Behaviors;
using BaseBuldingsBlocks.Middleware;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TodoTypeService.Data;
using TodoTypeService.Endpoints;
using FluentValidation;
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