using ProjectLothal.CustomDispatcher.Api.Commands;
using ProjectLothal.CustomDispatcher.Api.Response;

namespace ProjectLothal.CustomDispatcher.Api.Dispatchers
{
    public class Mediator(IServiceProvider provider)
    {
        public Result Dispatch(ICommand command)
        {
            Type type = typeof(ICommandHandler<>);
            Type[] typeArgs = [command.GetType()];
            Type handlerType = type.MakeGenericType(typeArgs);
            using var scope = provider.CreateScope();
            dynamic handler = scope.ServiceProvider.GetRequiredService(handlerType);
            Result result = handler.Handle((dynamic)command);

            return result;

        }
    }
}
