using Lothal.Mediator.Core.Dispatchers;
using Microsoft.AspNetCore.Mvc;
using ProjectLothal.CustomDispatcher.Api.Commands;
using ProjectLothal.CustomDispatcher.Api.DTOs.Basket;

namespace ProjectLothal.CustomDispatcher.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BasketController(Mediator mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AddProductBasket([FromBody] AddProductBasketDto dto)
        {
            var command = new AddProductBasketCommand()
            {
                Sku=dto.Sku,
                Qty=dto.Qty,
                Barcode=dto.Barcode,
                Color=dto.Color,
                Name=dto.Name,
            };
            var result = await mediator.Send(command);

            return Ok();
        }
    }

    
}