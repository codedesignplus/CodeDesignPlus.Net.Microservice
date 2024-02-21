using CodeDesignPlus.Net.Microservice.Application.Order.Commands.AddProductToOrder;
using CodeDesignPlus.Net.Microservice.Application.Order.Commands.CancelOrder;
using CodeDesignPlus.Net.Microservice.Application.Order.Commands.CompleteOrder;
using CodeDesignPlus.Net.Microservice.Application.Order.Commands.CreateOrder;
using CodeDesignPlus.Net.Microservice.Application.Order.Commands.RemoveProduct;
using CodeDesignPlus.Net.Microservice.Application.Order.Commands.UpdateQuantityProduct;
using CodeDesignPlus.Net.Microservice.Application.Order.Queries.FindOrderById;
using CodeDesignPlus.Net.Microservice.Application.Order.Queries.GetAllOrders;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CodeDesignPlus.Net.Microservice.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator mediator;

        public OrderController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrder([FromQuery] FindOrderByIdQuery data, CancellationToken cancellationToken)
        {
            var result = await this.mediator.Send(data, cancellationToken);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand data, CancellationToken cancellationToken)
        {
            await this.mediator.Send(data, cancellationToken);

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> AddProductToOrder([FromBody] AddProductToOrderCommand data, CancellationToken cancellationToken)
        {
            await mediator.Send(data, cancellationToken);

            return NoContent();
        }

        [HttpPatch]
        public async Task<IActionResult> CancelOrder([FromBody] CancelOrderCommand data, CancellationToken cancellationToken)
        {
            await mediator.Send(data, cancellationToken);

            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateQuantityProduct([FromBody] UpdateQuantityProductCommand data, CancellationToken cancellationToken)
        {
            await mediator.Send(data, cancellationToken);

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveProduct([FromBody] RemoveProductCommand data, CancellationToken cancellationToken)
        {
            await mediator.Send(data, cancellationToken);

            return NoContent();
        }

        [HttpPatch]
        public async Task<IActionResult> CompleteOrder([FromBody] CompleteOrderCommand data, CancellationToken cancellationToken)
        {
            await mediator.Send(data, cancellationToken);

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders(CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetAllOrdersQuery(), cancellationToken);

            return Ok(result);
        }
    }
}
