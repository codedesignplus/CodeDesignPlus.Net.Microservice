namespace CodeDesignPlus.Net.Microservice.Rest.Controllers;

/// <summary>
/// Controller class responsible for handling HTTP requests related to orders.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="OrdersController"/> class.
/// </remarks>
/// <param name="mediator">Mediator instance for sending commands and queries.</param>
/// <param name="mapper">Mapper instance for mapping between DTOs and commands/queries.</param>
[Route("api/[controller]")]
[ApiController]
public class OrdersController(IMediator mediator, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Retrieves a list of orders based on the specified criteria.
    /// </summary>
    /// <param name="criteria">Criteria object used for filtering orders.</param>
    /// <param name="cancellationToken">Cancellation token (optional).</param>
    /// <returns>An `OkObjectResult` containing the list of orders if successful, otherwise an appropriate error response.</returns>
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetOrders([FromQuery] C.Criteria criteria, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllOrdersQuery(criteria), cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Retrieves a specific order by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the order.</param>
    /// <param name="cancellationToken">Cancellation token (optional).</param>
    /// <returns>An `OkObjectResult` containing the order details if successful, otherwise an appropriate error response.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new FindOrderByIdQuery(id), cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Creates a new order.
    /// </summary>
    /// <param name="data">A DTO containing the order details.</param>
    /// <param name="cancellationToken">Cancellation token (optional).</param>
    /// <returns>A `NoContentResult` indicating successful creation, or an appropriate error response.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto data, CancellationToken cancellationToken)
    {
        await mediator.Send(mapper.Map<CreateOrderCommand>(data), cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Cancels an existing order.
    /// </summary>
    /// <param name="id">The unique identifier of the order to cancel.</param>
    /// <param name="data">A DTO containing cancellation details (may not be required depending on implementation).</param>
    /// <param name="cancellationToken">Cancellation token (optional).</param>
    /// <returns>A `NoContentResult` indicating successful cancellation, or an appropriate error response.</returns>
    [HttpDelete("{id}/cancel")]
    public async Task<IActionResult> CancelOrder(Guid id, [FromBody] CancelOrderDto data, CancellationToken cancellationToken)
    {
        data.Id = id;

        await mediator.Send(mapper.Map<CancelOrderCommand>(data), cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Marks an existing order as complete.
    /// </summary>
    /// <param name="id">The unique identifier of the order to complete.</param>
    /// <param name="cancellationToken">Cancellation token (optional).</param>
    /// <returns>A `NoContentResult` indicating successful completion, or an appropriate error response.</returns>
    [HttpPatch("{id}/complete")]
    public async Task<IActionResult> CompleteOrder(Guid id, CancellationToken cancellationToken)
    {
        await mediator.Send(new CompleteOrderCommand(id), cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Adds a product to an existing order.
    /// </summary>
    /// <param name="id">The unique identifier of the order to add the product to.</param>
    /// <param name="data">A DTO containing the product details and the quantity to add.</param>
    /// <param name="cancellationToken">Cancellation token (optional).</param>
    /// <returns>A `NoContentResult` indicating successful addition, or an appropriate error response.</returns>
    [HttpPost("{id}/products")]
    public async Task<IActionResult> AddProductToOrder(Guid id, [FromBody] AddProductToOrderDto data, CancellationToken cancellationToken)
    {
        data.Id = id;

        await mediator.Send(mapper.Map<AddProductToOrderCommand>(data), cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Updates the quantity of a product within an existing order.
    /// </summary>
    /// <param name="id">The unique identifier of the order containing the product.</param>
    /// <param name="productId">The unique identifier of the product to update the quantity for.</param>
    /// <param name="data">A DTO containing the new quantity for the product.</param>
    /// <param name="cancellationToken">Cancellation token (optional).</param>
    /// <returns>A `NoContentResult` indicating successful update, or an appropriate error response.</returns>
    [HttpPut("{id}/products/{productId}")]
    public async Task<IActionResult> UpdateQuantityProduct(Guid id, Guid productId, [FromBody] UpdateQuantityProductDto data, CancellationToken cancellationToken)
    {
        data.Id = id;
        data.ProductId = productId;

        await mediator.Send(mapper.Map<UpdateQuantityProductCommand>(data), cancellationToken);

        return NoContent();
    }

    /// <summary>
    /// Removes a product from an existing order.
    /// </summary>
    /// <param name="id">The unique identifier of the order containing the product.</param>
    /// <param name="productId">The unique identifier of the product to remove.</param>
    /// <param name="cancellationToken">Cancellation token (optional).</param>
    /// <returns>A `NoContentResult` indicating successful removal, or an appropriate error response.</returns>
    [HttpDelete("{id}/products/{productId}")]
    public async Task<IActionResult> RemoveProduct(Guid id, Guid productId, CancellationToken cancellationToken)
    {
        await mediator.Send(new RemoveProductCommand(id, productId), cancellationToken);

        return NoContent();
    }
}
