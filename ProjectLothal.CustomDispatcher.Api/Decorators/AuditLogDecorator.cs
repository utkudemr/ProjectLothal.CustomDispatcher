using Newtonsoft.Json;
using ProjectLothal.CustomDispatcher.Api.Commands;
using ProjectLothal.CustomDispatcher.Api.Dispatchers;
using ProjectLothal.CustomDispatcher.Api.Response;

namespace ProjectLothal.CustomDispatcher.Api.Decorators
{
    public class AuditLogDecorator<TCommand> : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        private readonly ICommandHandler<TCommand> _handler;
        private readonly ILoggerService _loggerService;
        public AuditLogDecorator(ICommandHandler<TCommand> handler, ILoggerService loggerService)
        {
            _handler = handler;
            _loggerService = loggerService;
        }
        public Result Handle(TCommand command)
        {
            var json = JsonConvert.SerializeObject(command);
            _loggerService.LogWarning($"Command of type {command.GetType().Name}: {json}");

            return _handler.Handle(command);
        }
    }
}
