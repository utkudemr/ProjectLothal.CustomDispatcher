using ProjectLothal.CustomDispatcher.Api.Commands;
using ProjectLothal.CustomDispatcher.Api.Dispatchers;
using System.Reflection;

namespace ProjectLothal.CustomDispatcher.Api
{
    public static class HandlerRegistration
    {
        public static void AddHandlers(this IServiceCollection services)
        {
            List<Type> handlerTypes = typeof(ICommand).Assembly.GetTypes()
                .Where(x => x.GetInterfaces().Any(y => IsHandlerInterface(y)))
                .Where(x => x.Name.EndsWith("Handler"))
                .ToList();

            handlerTypes.ForEach(type => RegisterHandler(services, type));
        }

        private static void RegisterHandler(IServiceCollection services, Type handlerType)
        {
            Type handlerInterface = handlerType.GetInterfaces().Single(IsHandlerInterface);
            Func<IServiceProvider, object> implementationFactory = BuildFactory(handlerType, handlerInterface);

            services.AddScoped(handlerInterface, implementationFactory);
        }


        private static Func<IServiceProvider, object> BuildFactory(Type handlerType, Type interfaceType)
        {
            var contructorMethod = handlerType.GetConstructors().Single();

            Func<IServiceProvider, object> constructorFunc = provider =>
            {
                var parameters = contructorMethod.GetParameters().ToList();
                var dependecies = GetHandlerDepedencies(parameters, provider);
                return contructorMethod.Invoke(dependecies);
            };

            return constructorFunc;
        }

        private static object?[] GetHandlerDepedencies(List<ParameterInfo> parameters, IServiceProvider provider)
        {
            var result = new object?[parameters.Count];

            for (int i = 0; i < parameters.Count; i++)
            {
                var parameter = parameters[i];
                Type parameterType = parameter.ParameterType;
                if (!parameterType.IsGenericType)
                {
                    var service = provider.GetService(parameterType);
                    if(service != null)
                    {
                        result[i] = service;
                    }
                }
            }

            return result;
        }

        private static bool IsHandlerInterface(Type type)
        {
            if (!type.IsGenericType)
                return false;

            Type typeDefinition = type.GetGenericTypeDefinition();

            return typeDefinition == typeof(ICommandHandler<>);
        }
    }
}
