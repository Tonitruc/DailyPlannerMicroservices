using TodoService.Application.Contracts.MessageBroker;
using TodoService.Application.UseCases.Todos.Events;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using TodoService.Infrastracture.Data.Repositories;
using TodoService.Infrastracture.Data.Interceptors;
using TodoService.Domain.Contracts.Repositories;
using Microsoft.Extensions.DependencyInjection;
using TodoService.Infrastracture.Contracts;
using Microsoft.Extensions.Configuration;
using TodoService.Infrastracture.Data;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using TodoService.Domain.Constants;
using TodoService.Domain.Events;
using MassTransit;
using System.Text;

namespace TodoService.Infrastracture;

public static class DependencyInjection
{
    public static void AddInfrastractureServices(this IHostApplicationBuilder applicationBuilder)
    {
        #region Database settings

        applicationBuilder.Services.AddSingleton(TimeProvider.System);
        applicationBuilder.Services.AddScoped<AuditableEntityInterceptor>();

        applicationBuilder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.UseNpgsql(applicationBuilder.Configuration.GetConnectionString("PostgresSQL"));

            var interseptor = sp.GetRequiredService<AuditableEntityInterceptor>();
            options.AddInterceptors(interseptor);
        });
        applicationBuilder.Services.AddScoped<ApplicationDatabaseInitializer>();

        applicationBuilder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
        applicationBuilder.Services.AddScoped<ITodoRepository, TodoRepository>();

        #endregion

        #region Message Broker

        applicationBuilder.Services.AddScoped<IDelayMessagePublisher, RabbitMqDelayMessagePublisher>();

        applicationBuilder.Services.AddMassTransit(cfg =>
        {
            cfg.AddConsumer<TodoExpiredEventHandler>();

            cfg.UsingRabbitMq((context, config) =>
            {
                var settings = applicationBuilder.Configuration.GetSection("RabbitMQSettings");

                config.Host(settings["Host"], options =>
                {
                    options.Username(settings["Username"]!);
                    options.Password(settings["Password"]!);
                });

                config.ConfigureEndpoints(context);

                config.Message<TodoExpiredEvent>(e =>
                {
                    e.SetEntityName("todo-expired-exchange");
                });

                config.Publish<TodoExpiredEvent>(x =>
                {
                    x.ExchangeType = "x-delayed-message";
                    x.SetExchangeArgument("x-delayed-type", "direct");
                });
            });
        });

        #endregion

        #region Authentication and authorization 

        applicationBuilder.Services.AddAuthentication(options =>
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
                        Encoding.UTF8.GetBytes(applicationBuilder.Configuration["JwtSettings:ActiveToken:SecurityKey"]!)),

                    ValidateAudience = false,
                    ValidateIssuer = false
                };
            });

        applicationBuilder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(Policies.AdminOnly, policy =>
                policy.RequireRole("Admin"));
        });

        #endregion
    }
}
