

using ProjectLothal.CustomDispatcher.Api.Commands;
using ProjectLothal.CustomDispatcher.Api.Response;

namespace ProjectLothal.CustomDispatcher.Api.Dispatchers
{
    public interface ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        Result Handle(TCommand command);
    }
}
