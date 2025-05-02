using ProjectLothal.CustomDispatcher.Api.Dispatchers;
using ProjectLothal.CustomDispatcher.Api.Response;
using ProjectLothal.CustomDispatcher.Api.Services;

namespace ProjectLothal.CustomDispatcher.Api.Commands
{
    public sealed class AddProductBasketCommand : IRequest<Result>
    {
        public int Sku { get; set; }
        public int Qty { get; set; }
        public string? Barcode { get; set; }
        public string? Name { get; set; }
        public string? Color { get; set; }
    }

    public sealed class AddProductBasketHandler(
        ILogger<AddProductBasketHandler> logger,
        ITestBusinessService testBusinessService)
        : IRequestHandler<AddProductBasketCommand, Result>
    {
        public Task<Result> Handle(AddProductBasketCommand request, CancellationToken cancellationToken)
        {
            testBusinessService.TestMethod();
            return Task.FromResult(new Result());
        }
    }
}