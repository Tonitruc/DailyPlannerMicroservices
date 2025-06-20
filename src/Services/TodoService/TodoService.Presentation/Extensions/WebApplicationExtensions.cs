using System.Reflection;

namespace TodoService.Presentation.Extensions;

public static class WebApplicationExtensions
{
    public static RouteGroupBuilder MapGroup(this WebApplication app, EndpointGroupBase group)
    {
        var groupName = group.GetType().Name;

        return app
            .MapGroup($"/api/{groupName}")
            .WithGroupName(groupName)
            .WithTags(groupName);
    }

    public static WebApplication MapEndpoints(this WebApplication app)
    {
        var groupType = typeof(EndpointGroupBase);

        var assembly = Assembly.GetExecutingAssembly();
        var endpointGroups = assembly.GetExportedTypes()
            .Where(t => t.IsSubclassOf(groupType));

        foreach (var type in endpointGroups)
        {
            if (Activator.CreateInstance(type) is EndpointGroupBase instance)
            {
                instance.Map(app);
            }
        }

        return app;
    }
}