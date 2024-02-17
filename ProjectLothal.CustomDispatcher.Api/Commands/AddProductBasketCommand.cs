
using ProjectLothal.CustomDispatcher.Api.Decorators;
using ProjectLothal.CustomDispatcher.Api.Dispatchers;
using ProjectLothal.CustomDispatcher.Api.Response;
using ProjectLothal.CustomDispatcher.Api.Services;

namespace ProjectLothal.CustomDispatcher.Api.Commands
{
    public sealed class AddProductBasketCommand: ICommand
    {
        public int Sku { get; set; }
        public int Qty { get; set; }
        public string? Barcode { get; set; }
        public string? Name { get; set; }
        public string? Color { get; set; }

        [AuditLog]
        public sealed class AddProductBasketHandler : ICommandHandler<AddProductBasketCommand>
        {
            private readonly ILogger<AddProductBasketHandler> _logger;
            private readonly ITestBusinessService _testBusinessService;


            public AddProductBasketHandler(ILogger<AddProductBasketHandler> logger, ITestBusinessService testBusinessService)
            {
                _logger = logger;
                _testBusinessService = testBusinessService;
            }

            public Result Handle(AddProductBasketCommand command)
            {
                _testBusinessService.TestMethod();
                return new Result();
            }

        }
    }
}


