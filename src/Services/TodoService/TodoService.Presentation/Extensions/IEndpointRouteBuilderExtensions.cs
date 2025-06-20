using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace TodoService.Presentation.Extensions;

public static class IEndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapGet(this IEndpointRouteBuilder endpointRouteBuilder, Delegate handler,
        [StringSyntax("Route")] string pattern = "")
    {
        if (handler.Method.IsAnonymous())
            throw new ArgumentException("The endpoint name must be specified when using anonymous handlers.");

        endpointRouteBuilder.MapGet(pattern, handler);

        return endpointRouteBuilder;
    }

    public static IEndpointRouteBuilder MapPost(this IEndpointRouteBuilder endpointRouteBuilder, Delegate handler,
        [StringSyntax("Route")] string pattern = "")
    {
        if (handler.Method.IsAnonymous())
            throw new ArgumentException("The endpoint name must be specified when using anonymous handlers.");

        endpointRouteBuilder.MapPost(pattern, handler);

        return endpointRouteBuilder;
    }

    public static IEndpointRouteBuilder MapPut(this IEndpointRouteBuilder endpointRouteBuilder, Delegate handler,
        [StringSyntax("Route")] string pattern = "")
    {
        if (handler.Method.IsAnonymous())
            throw new ArgumentException("The endpoint name must be specified when using anonymous handlers.");

        endpointRouteBuilder.MapPut(pattern, handler);

        return endpointRouteBuilder;
    }

    public static IEndpointRouteBuilder MapDelete(this IEndpointRouteBuilder endpointRouteBuilder, Delegate handler,
        [StringSyntax("Route")] string pattern = "")
    {
        if (handler.Method.IsAnonymous())
            throw new ArgumentException("The endpoint name must be specified when using anonymous handlers.");

        endpointRouteBuilder.MapDelete(pattern, handler);

        return endpointRouteBuilder;
    }

    private static bool IsAnonymous(this MethodInfo method)
    {
        var invalidChars = new[] { '<', '>' };
        return method.Name.Any(ch => invalidChars.Contains(ch));
    }
}
