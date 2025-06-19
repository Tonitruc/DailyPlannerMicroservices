using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace TodoService.Application;

public static class DependencyInjection
{
    public static void AddApplicationServices(this IHostApplicationBuilder applicationBuilder)
    {
        applicationBuilder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

        applicationBuilder.Services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
        });
    }
}
