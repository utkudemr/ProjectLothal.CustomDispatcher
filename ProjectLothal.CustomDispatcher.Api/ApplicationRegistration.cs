using ProjectLothal.CustomDispatcher.Api.Commands;
using ProjectLothal.CustomDispatcher.Api.Decorators;
using ProjectLothal.CustomDispatcher.Api.Dispatchers;
using System.Reflection;

namespace ProjectLothal.CustomDispatcher.Api
{
    public static class HandlerRegistration
    {
        public static void AddHandlers(this IServiceCollection services)
        {
            var handlerTypes = GetHandlerTypes();

            foreach (var handlerType in handlerTypes)
            {
                var handlerInterface = handlerType.GetInterfaces().First(i => i.GetGenericTypeDefinition() == typeof(ICommandHandler<>));
                var genericArgs = handlerInterface.GetGenericArguments();

                var attributes = handlerType.GetCustomAttributes<BaseAttribute>().ToList();
                var decoratorTypes = attributes
                    .Select(a => a.GetDecoratorType().MakeGenericType(genericArgs))
                    .ToList();

                var typeChain = decoratorTypes.Append(handlerType).Reverse().ToList();

                services.AddScoped(handlerInterface, sp =>
                {
                    object? current = null;

                    foreach (var type in typeChain)
                    {
                        var ctor = type.GetConstructors().Single();
                        var args = ctor.GetParameters().Select(p =>
                            p.ParameterType.IsAssignableFrom(handlerInterface) ? current : sp.GetRequiredService(p.ParameterType)
                        ).ToArray();

                        current = Activator.CreateInstance(type, args);
                    }

                    return current!;
                });
            }
        }

        private static List<Type> GetHandlerTypes()
        {
            return typeof(ICommand).Assembly.GetTypes()
                .Where(x => x.GetInterfaces().Any(IsHandlerInterface))
                .ToList();
        }

        private static bool IsHandlerInterface(Type type)
        {
            if (!type.IsGenericType)
                return false;

            var typeDefinition = type.GetGenericTypeDefinition();

            return typeDefinition == typeof(ICommandHandler<>);
        }
    }
}
