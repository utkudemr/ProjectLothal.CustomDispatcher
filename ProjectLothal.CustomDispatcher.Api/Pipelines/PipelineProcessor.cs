using ProjectLothal.CustomDispatcher.Api.Dispatchers;

namespace ProjectLothal.CustomDispatcher.Api.Pipelines;

public class PipelineProcessor(IServiceProvider serviceProvider)
{
    public async Task<TResponse> Execute<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken)
    {
        try
        {
            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
            dynamic handler = serviceProvider.GetRequiredService(handlerType);

            var behaviorType = typeof(IPipelineBehavior<,>).MakeGenericType(request.GetType(), typeof(TResponse));
            var behaviors = serviceProvider.GetServices(behaviorType).Cast<dynamic>().Reverse().ToList();

            RequestHandlerDelegate<TResponse> handlerDelegate = () => handler.Handle((dynamic)request, cancellationToken);

            foreach (var behavior in behaviors)
            {
                var next = handlerDelegate;
                handlerDelegate = () => behavior.Handle((dynamic)request, cancellationToken, next);
            }

            return await handlerDelegate();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
     
    }
}