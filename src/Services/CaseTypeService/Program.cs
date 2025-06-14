using CaseTypeService.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//________________ Database ________________
builder.Services.AddSingleton<ApplicationDatabaseInitializer>();
builder.Services.AddDbContext<ApplicationDbContext>((sp, options) => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQL"))
           .AddSeedingAsync(sp));

app.MapGet("/", () => "Case type service is work...");

app.MapGet("/api/v1/case-types", async (ApplicationDbContext context) =>
     await context.CaseTypes.ToListAsync());

//________________ Database initialization ________________
await app.InitializeAsync();

app.Run();
