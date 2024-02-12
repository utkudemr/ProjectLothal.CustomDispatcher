using Microsoft.AspNetCore.Mvc;
using ProjectLothal.CustomDispatcher.Api.Commands;
using ProjectLothal.CustomDispatcher.Api.Dispatchers;
using ProjectLothal.CustomDispatcher.Api.DTOs.Basket;

namespace ProjectLothal.CustomDispatcher.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly Mediator _mediator;
        public BasketController(Mediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public IActionResult AddProductBasket([FromBody] AddProductBasketDto dto)
        {
            var command = new AddProductBasketCommand()
            {
                Sku=dto.Sku,
                Qty=dto.Qty,
                Barcode=dto.Barcode,
                Color=dto.Color,
                Name=dto.Name,
            };

            var result = _mediator.Dispatch(command);
            return Ok();
        }
    }

    
}