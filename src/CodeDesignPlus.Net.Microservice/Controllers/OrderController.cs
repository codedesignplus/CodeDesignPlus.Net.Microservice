using CodeDesignPlus.Net.Microservice.Application.Order.Commands.AddProductToOrder;
using CodeDesignPlus.Net.Microservice.Application.Order.Commands.CancelOrder;
using CodeDesignPlus.Net.Microservice.Application.Order.Commands.CompleteOrder;
using CodeDesignPlus.Net.Microservice.Application.Order.Commands.CreateOrder;
using CodeDesignPlus.Net.Microservice.Application.Order.Commands.RemoveProduct;
using CodeDesignPlus.Net.Microservice.Application.Order.Commands.UpdateQuantityProduct;
using CodeDesignPlus.Net.Microservice.Application.Order.Queries.FindOrderById;
using CodeDesignPlus.Net.Microservice.Application.Order.Queries.GetAllOrders;
using CodeDesignPlus.Microservice.Api.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace CodeDesignPlus.Net.Microservice.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public OrderController(IMediator mediator, IMapper mapper)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrder([FromQuery] FindOrderByIdQuery data, CancellationToken cancellationToken)
        {
            var result = await this.mediator.Send(data, cancellationToken);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto data, CancellationToken cancellationToken)
        {
            await this.mediator.Send(this.mapper.Map<CreateOrderCommand>(data), cancellationToken);

            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> AddProductToOrder([FromBody] AddProductToOrderDto data, CancellationToken cancellationToken)
        {
            await mediator.Send(this.mapper.Map<AddProductToOrderCommand>(data), cancellationToken);

            return NoContent();
        }

        [HttpPatch]
        public async Task<IActionResult> CancelOrder([FromBody] CancelOrderDto data, CancellationToken cancellationToken)
        {
            await mediator.Send(this.mapper.Map<CancelOrderCommand>(data), cancellationToken);

            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateQuantityProduct([FromBody] UpdateQuantityProductDto data, CancellationToken cancellationToken)
        {
            await mediator.Send(this.mapper.Map<UpdateQuantityProductCommand>(data), cancellationToken);

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveProduct([FromBody] RemoveProductDto data, CancellationToken cancellationToken)
        {
            await mediator.Send(this.mapper.Map<RemoveProductCommand>(data), cancellationToken);

            return NoContent();
        }

        [HttpPatch]
        public async Task<IActionResult> CompleteOrder([FromBody] CompleteOrderDto data, CancellationToken cancellationToken)
        {
            await mediator.Send(this.mapper.Map<CompleteOrderCommand>(data), cancellationToken);

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
