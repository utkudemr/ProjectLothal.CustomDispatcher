using ProjectLothal.CustomDispatcher.Api.Dispatchers;
using System.Reflection;
using ProjectLothal.CustomDispatcher.Api.Pipelines;

namespace ProjectLothal.CustomDispatcher.Api;

public static class HandlerRegistration
{
    public static void AddHandlers(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var handlerTypes = assembly.GetTypes()
            .Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType &&
                (i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
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