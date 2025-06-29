using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace TodoService.Presentation.Extensions;

public static class IEndpointRouteBuilderExtensions
{
    public static RouteHandlerBuilder MapGet(this IEndpointRouteBuilder endpointRouteBuilder, Delegate handler,
        [StringSyntax("Route")] string pattern = "")
    {
        if (handler.Method.IsAnonymous())
            throw new ArgumentException("The endpoint name must be specified when using anonymous handlers.");

        var routeHandlerBuilder = endpointRouteBuilder.MapGet(pattern, handler);

        return routeHandlerBuilder;
    }

    public static RouteHandlerBuilder MapPost(this IEndpointRouteBuilder endpointRouteBuilder, Delegate handler,
        [StringSyntax("Route")] string pattern = "")
    {
        if (handler.Method.IsAnonymous())
            throw new ArgumentException("The endpoint name must be specified when using anonymous handlers.");

        var routeHandlerBuilder = endpointRouteBuilder.MapPost(pattern, handler);

        return routeHandlerBuilder;
    }

    public static RouteHandlerBuilder MapPut(this IEndpointRouteBuilder endpointRouteBuilder, Delegate handler,
        [StringSyntax("Route")] string pattern = "")
    {
        if (handler.Method.IsAnonymous())
            throw new ArgumentException("The endpoint name must be specified when using anonymous handlers.");

        var routeHandlerBuilder = endpointRouteBuilder.MapPut(pattern, handler);

        return routeHandlerBuilder;
    }

    public static RouteHandlerBuilder MapDelete(this IEndpointRouteBuilder endpointRouteBuilder, Delegate handler,
        [StringSyntax("Route")] string pattern = "")
    {
        if (handler.Method.IsAnonymous())
            throw new ArgumentException("The endpoint name must be specified when using anonymous handlers.");

        var routeHandlerBuilder = endpointRouteBuilder.MapDelete(pattern, handler);

        return routeHandlerBuilder;
    }

    private static bool IsAnonymous(this MethodInfo method)
    {
        var invalidChars = new[] { '<', '>' };
        return method.Name.Any(ch => invalidChars.Contains(ch));
    }
}
