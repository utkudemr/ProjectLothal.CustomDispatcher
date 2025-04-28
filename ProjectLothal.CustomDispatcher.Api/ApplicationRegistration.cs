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
            handlerTypes.ForEach(type => RegisterHandler(services, type));
        }

        private static List<Type> GetHandlerTypes()
        {
            return typeof(ICommand).Assembly.GetTypes()
                .Where(x => x.GetInterfaces().Any(IsHandlerInterface))
                .ToList();
        }

        private static void RegisterHandler(IServiceCollection services, Type handlerType)
        {
            Type handlerInterface = handlerType.GetInterfaces().Single(IsHandlerInterface);
            
            var handlerDecoratorTypes = HandlerDecoratorTypes(handlerType);

            var handlerTypeWithDecorators = handlerDecoratorTypes
             .Concat([handlerType])
             .Reverse() //Handler Type first
             .ToList();

            Func<IServiceProvider, object> implementationFactory = BuildFactory(handlerTypeWithDecorators, handlerInterface);

            services.AddScoped(handlerInterface, implementationFactory);
        }

        private static List<Type> HandlerDecoratorTypes(Type handlerType)
        {
            var handlerDecorators = handlerType.GetCustomAttributes(false);
            var handlerDecoratorTypes = handlerDecorators.Where(a => a.GetType().BaseType == typeof(BaseAttribute)).Select(ToDecorator).ToList();
            return handlerDecoratorTypes;
        }
        

        private static Func<IServiceProvider, object> BuildFactory(List<Type> handlerTypes, Type interfaceType)
        {
            handlerTypes = handlerTypes.Select(handlerType => MakeGenericHandler(handlerType, interfaceType)).ToList();

           
            var constructorMethods = handlerTypes.Select(a=>a.GetConstructors().Single()).ToList();

            Func<IServiceProvider, object> constructorFunc = provider =>
            {
                object? currentType = null; //First set handler Constructor and then connect attribute with handler this parameter.
                foreach (var constructorMethod in constructorMethods)
                {
                    var parameters = constructorMethod.GetParameters().ToList();
                    var dependecies = GetHandlerDepedencies(parameters, currentType, provider);
                    currentType = constructorMethod.Invoke(dependecies);
                }

                if(currentType == null) { throw new ArgumentNullException(nameof(currentType)); }

                return currentType;
            };

            return constructorFunc;
        }

        private static Type MakeGenericHandler(Type handlerType, Type interfaceType)
        {
            return handlerType.IsGenericType ? (handlerType.MakeGenericType(interfaceType.GenericTypeArguments)) : handlerType;
        }

        private static object?[] GetHandlerDepedencies(List<ParameterInfo> parameters, object? currentType, IServiceProvider provider)
        {
            var result = new object?[parameters.Count];

            for (int i = 0; i < parameters.Count; i++)
            {
                var parameter = parameters[i];
                result[i] = GetHandlerDepedency(parameter,currentType,provider);
            }
            return result;
        }

        private static object? GetHandlerDepedency(ParameterInfo parameter, object? currentType, IServiceProvider provider)
        {
            Type parameterType = parameter.ParameterType;
            if (parameterType.IsGenericType) return currentType;
            var service = provider.GetService(parameterType);
            if (service != null)
            {
                return service;
            }
            throw new ArgumentNullException(nameof(parameter));
        }

        private static bool IsHandlerInterface(Type type)
        {
            if (!type.IsGenericType)
                return false;

            var typeDefinition = type.GetGenericTypeDefinition();

            return typeDefinition == typeof(ICommandHandler<>);
        }

        private static Type ToDecorator(object attribute)
        {
            Type type = attribute.GetType();

            if (type == typeof(AuditLogAttribute))
                return typeof(AuditLogDecorator<>);

            throw new ArgumentException(attribute.ToString());
        }
    }
}
