using System.Reflection;
using Lothal.Mediator.Core.Dispatchers;
using Lothal.Mediator.Core.Pipelines;
using Microsoft.Extensions.DependencyInjection;

namespace Lothal.Mediator.Core;

public static class HandlerRegistration
{
    public static void AddHandlers(this IServiceCollection services, Assembly assembly)
    {
        var handlerTypes = assembly.GetTypes()
            .Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)
            ))
            .ToList();

        foreach (var handler in handlerTypes)
        {
            var interfaceType = handler.GetInterfaces().First(i =>
                (i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)));

            services.AddScoped(interfaceType, handler);
        }

        services.AddScoped<PipelineProcessor>();
    }
}