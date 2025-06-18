using TodoTypeService.Data;
using TodoTypeService.Endpoints;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

//________________ Database ________________
builder.Services.AddScoped<ApplicationDatabaseInitializer>();
builder.Services.AddDbContext<ApplicationDbContext>((sp, options) => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresSQL"))
           .AddSeedingAsync(sp));

//________________ MediatR ________________
builder.Services.AddMediatR(options =>
    options.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

//________________ AutoMapper ________________
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

var app = builder.Build();

//________________ Endpoints ________________
app.MapGet("/", () => "Todo type service is work...");
app.MapTodoTypes();

//________________ Database initialization ________________
await app.InitializeAsync();

app.Run();