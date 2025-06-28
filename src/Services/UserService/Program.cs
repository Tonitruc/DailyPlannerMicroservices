using Microsoft.AspNetCore.Authentication.JwtBearer;
using UserService.UseCases.Services.Authentication;
using UserService.UseCases.Services.UserClaims;
using UserService.UseCases.Services.UserRead;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BaseBuldingsBlocks.Middleware;
using BaseBuldingsBlocks.Behaviors;
using UserService.Endpoints;
using UserService.Models;
using System.Reflection;
using UserService.Data;
using FluentValidation;
using System.Text;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

#region Databse and user settings

builder.Services.AddScoped<ApplicationDatabaseInitializer>();
builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresSQL"))
           .AddSeedingAsync(sp));

builder.Services.AddIdentityCore<User>(options =>
    {
        options.Password.RequiredLength = 4;
        options.Password.RequiredUniqueChars = 0;
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.Configure<JwtTokensSettings>(
    builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:ActiveToken:SecurityKey"]!)),

            ValidateAudience = false,  
            ValidateIssuer = false
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddScoped<JwtClaimsMiddleware>();

builder.Services.AddScoped<IAuthenticateManager, AuthenticateManager>();

#endregion

#region MediatR

builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

    options.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
});

#endregion

#region Other Services 

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddScoped<IUserReadService, UserReadService>();
builder.Services.AddScoped<IUserClaimsService, UserClaimsService>();

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

#endregion

var app = builder.Build();

app.UseExceptionHandler(options => { });

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<JwtClaimsMiddleware>();

await app.InitializeDatabaseAsync();

#region Endpoints

app.MapGet("/", () => "User service is work...!");
app.MapUserEndpoints();
app.MapAuthenticationEndpoints();

#endregion

app.Run();
