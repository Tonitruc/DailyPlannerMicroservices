using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TodoService.Infrastracture.Data;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using TodoService.Domain.Contracts.Repositories;
using TodoService.Infrastracture.Data.Repositories;

namespace TodoService.Infrastracture;

public static class DependencyInjection
{
    public static void AddInfrastractureServices(this IHostApplicationBuilder hostBuilder)
    {
        hostBuilder.Services.AddSingleton(TimeProvider.System);

        hostBuilder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(hostBuilder.Configuration.GetConnectionString("PostgresSQL")));
        hostBuilder.Services.AddScoped<ApplicationDatabaseInitializer>();

        hostBuilder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
        hostBuilder.Services.AddScoped<ITodoRepository, TodoRepository>();
    }
}
