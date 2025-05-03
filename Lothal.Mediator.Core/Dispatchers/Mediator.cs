using Lothal.Mediator.Core.Pipelines;
using Microsoft.Extensions.DependencyInjection;

namespace Lothal.Mediator.Core.Dispatchers;

public class Mediator
{
    private readonly IServiceProvider _provider;

    public Mediator(IServiceProvider provider)
    {
        _provider = provider;
    }

    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request,
        CancellationToken cancellationToken = default)
    {
        using var scope = _provider.CreateScope();

        var executor = scope.ServiceProvider.GetRequiredService<PipelineProcessor>();

        return await executor.Execute(request, cancellationToken);
    }
}