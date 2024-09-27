using CodeDesignPlus.Net.Microservice.Application.Order.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Application.Order.Queries.FindOrderById;
using CodeDesignPlus.Net.Microservice.gRpc.Services;
using Grpc.Core;
using Grpc.Core.Testing;

namespace CodeDesignPlus.Net.Microservice.gRpc.Test.Services;

public class OrderServiceTest
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly OrdersService _ordersService;

    public OrderServiceTest()
    {
        _mediatorMock = new Mock<IMediator>();
        _mapperMock = new Mock<IMapper>();
        _ordersService = new OrdersService(_mediatorMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetOrder_ValidId_ReturnsOrder()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var orderDto = new OrderDto
        {
            Id = orderId,
            Products = []
        };
        var order = new Order
        {
            Id = orderId.ToString()
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<FindOrderByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(orderDto);

        _mapperMock.Setup(m => m.Map<OrderDto, Order>(It.IsAny<OrderDto>())).Returns(order);
        _mapperMock.Setup(m => m.Map<ProductDto, Product>(It.IsAny<ProductDto>())).Returns(new Product());

        var requestStream = new Mock<IAsyncStreamReader<GetOrderRequest>>();
        requestStream.SetupSequence(r => r.MoveNext(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true)
            .ReturnsAsync(false);
        requestStream.Setup(r => r.Current).Returns(new GetOrderRequest { Id = orderId.ToString() });

        var responseStream = new Mock<IServerStreamWriter<GetOrderResponse>>();
        var serverCallContext = TestServerCallContext.Create("GetOrder", null, DateTime.UtcNow, [], CancellationToken.None, "127.0.0.1", null, null, null, null, null);

        // Act
        await _ordersService.GetOrder(requestStream.Object, responseStream.Object, serverCallContext);

        // Assert
        responseStream.Verify(r => r.WriteAsync(It.Is<GetOrderResponse>(response => response.Order.Id == orderId.ToString())), Times.Once);
    }

    [Fact]
    public async Task GetOrder_InvalidId_ThrowsRpcException()
    {
        // Arrange
        var requestStream = new Mock<IAsyncStreamReader<GetOrderRequest>>();
        requestStream.SetupSequence(r => r.MoveNext(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true)
            .ReturnsAsync(false);
        requestStream.Setup(r => r.Current).Returns(new GetOrderRequest { Id = "invalid-guid" });

        var responseStream = new Mock<IServerStreamWriter<GetOrderResponse>>();
        var serverCallContext = TestServerCallContext.Create("GetOrder", null, DateTime.UtcNow, [], CancellationToken.None, "127.0.0.1", null, null, null, null, null);

        // Act & Assert
        await Assert.ThrowsAsync<RpcException>(() => _ordersService.GetOrder(requestStream.Object, responseStream.Object, serverCallContext));
    }
}



