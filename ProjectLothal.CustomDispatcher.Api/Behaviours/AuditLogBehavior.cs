using Lothal.Mediator.Core.Dispatchers;
using Lothal.Mediator.Core.Pipelines;
using Newtonsoft.Json;

namespace ProjectLothal.CustomDispatcher.Api.Decorators;

public class AuditLogBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {
        var requestJson = JsonConvert.SerializeObject(request);
        Console.WriteLine($"Request {typeof(TRequest).Name} Json: {requestJson}");
        var requestResponse = await next();
        Console.WriteLine($"Response {typeof(TRequest).Name} Json: {requestResponse}");
        return requestResponse;
    }
}