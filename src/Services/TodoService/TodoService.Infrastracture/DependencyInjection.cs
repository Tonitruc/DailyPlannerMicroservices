using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using TodoService.Application.UseCases.Todos.Events;
using TodoService.Domain.Contracts.Repositories;
using TodoService.Infrastracture.Data;
using TodoService.Infrastracture.Data.Repositories;

namespace TodoService.Infrastracture;

public static class DependencyInjection
{
    public static void AddInfrastractureServices(this IHostApplicationBuilder applicationBuilder)
    {
        applicationBuilder.Services.AddSingleton(TimeProvider.System);

        applicationBuilder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(applicationBuilder.Configuration.GetConnectionString("PostgresSQL")));
        applicationBuilder.Services.AddScoped<ApplicationDatabaseInitializer>();

        applicationBuilder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
        applicationBuilder.Services.AddScoped<ITodoRepository, TodoRepository>();

        applicationBuilder.Services.AddMassTransit(x =>
        {
            x.AddConsumer(typeof(TestEventHandler));

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ConfigureEndpoints(context);
            });
        });
    }
}
