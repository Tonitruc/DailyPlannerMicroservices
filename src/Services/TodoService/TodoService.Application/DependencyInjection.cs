using Microsoft.Extensions.DependencyInjection;
using BaseBuldingsBlocks.Behaviors;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using FluentValidation;
using MediatR;

namespace TodoService.Application;

public static class DependencyInjection
{
    public static void AddApplicationServices(this IHostApplicationBuilder applicationBuilder)
    {
        applicationBuilder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

        applicationBuilder.Services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());

            options.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        applicationBuilder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
