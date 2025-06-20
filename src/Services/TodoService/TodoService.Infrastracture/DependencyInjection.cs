using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TodoService.Infrastracture.Data;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using TodoService.Domain.Contracts.Repositories;
using TodoService.Infrastracture.Data.Repositories;
using FluentValidation;
using System.Reflection;

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
    }
}
