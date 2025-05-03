namespace Lothal.Mediator.Core.Pipelines;

public interface IPipelineBehavior<in TRequest, TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next);
}

public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();