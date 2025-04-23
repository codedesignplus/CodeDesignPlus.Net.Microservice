using CodeDesignPlus.Net.Core.Abstractions.Models.Pager;
using CodeDesignPlus.Net.Microservice.Application.Order.Commands.AddProductToOrder;
using CodeDesignPlus.Net.Microservice.Application.Order.Commands.CancelOrder;
using CodeDesignPlus.Net.Microservice.Application.Order.Commands.CompleteOrder;
using CodeDesignPlus.Net.Microservice.Application.Order.Commands.CreateOrder;
using CodeDesignPlus.Net.Microservice.Application.Order.Commands.RemoveProduct;
using CodeDesignPlus.Net.Microservice.Application.Order.Commands.UpdateQuantityProduct;
using CodeDesignPlus.Net.Microservice.Application.Order.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Application.Order.Queries.FindOrderById;
using CodeDesignPlus.Net.Microservice.Application.Order.Queries.GetAllOrders;

namespace CodeDesignPlus.Net.Microservice.Rest.Test.Controllers;

public class OrderControllerTest
{
    private readonly Mock<IMediator> mediator;
    private readonly Mock<IMapper> mapper;

    public OrderControllerTest()
    {
        this.mediator = new Mock<IMediator>();
        this.mapper = new Mock<IMapper>();
    }

    [Fact]
    public async Task GetOrdersAsync_WhenCalled_ReturnsOk()
    {
        // Arrange
        var controller = new OrdersController(this.mediator.Object, this.mapper.Object);
        var criteria = new C.Criteria();
        var cancellationToken = new CancellationToken();

        this.mediator
            .Setup(x => x.Send(It.IsAny<GetAllOrdersQuery>(), cancellationToken))
            .ReturnsAsync(Pagination<OrderDto>.Create([], 0, 0, 0));

        // Act
        var result = await controller.GetOrders(criteria, cancellationToken);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        this.mediator.Verify(x => x.Send(It.IsAny<GetAllOrdersQuery>(), cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetOrderByIdAsync_WhenCalled_ReturnsOk()
    {
        // Arrange
        var controller = new OrdersController(this.mediator.Object, this.mapper.Object);
        var id = Guid.NewGuid();
        var cancellationToken = new CancellationToken();

        this.mediator
            .Setup(x => x.Send(It.IsAny<FindOrderByIdQuery>(), cancellationToken))
            .ReturnsAsync(new OrderDto());

        // Act
        var result = await controller.GetOrderById(id, cancellationToken);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        this.mediator.Verify(x => x.Send(It.IsAny<FindOrderByIdQuery>(), cancellationToken), Times.Once);
    }

    [Fact]
    public async Task CreateOrderAsync_WhenCalled_ReturnsNoContent()
    {
        // Arrange
        var controller = new OrdersController(this.mediator.Object, this.mapper.Object);
        var data = new CodeDesignPlus.Microservice.Api.Dtos.CreateOrderDto();
        var cancellationToken = new CancellationToken();

        this.mediator
            .Setup(x => x.Send(It.IsAny<CreateOrderCommand>(), cancellationToken))
            .Returns(Task.CompletedTask);

        // Act
        var result = await controller.CreateOrder(data, cancellationToken);

        // Assert
        Assert.IsType<NoContentResult>(result);
        this.mediator.Verify(x => x.Send(It.IsAny<CreateOrderCommand>(), cancellationToken), Times.Once);
    }

    [Fact]
    public async Task CancelOrderAsync_WhenCalled_ReturnsNoContent()
    {
        // Arrange
        var controller = new OrdersController(this.mediator.Object, this.mapper.Object);
        var id = Guid.NewGuid();
        var data = new CodeDesignPlus.Microservice.Api.Dtos.CancelOrderDto();
        var cancellationToken = new CancellationToken();

        this.mediator
            .Setup(x => x.Send(It.IsAny<CancelOrderCommand>(), cancellationToken))
            .Returns(Task.CompletedTask);

        // Act
        var result = await controller.CancelOrder(id, data, cancellationToken);

        // Assert
        Assert.IsType<NoContentResult>(result);
        this.mediator.Verify(x => x.Send(It.IsAny<CancelOrderCommand>(), cancellationToken), Times.Once);
    }

    [Fact]
    public async Task CompleteOrderAsync_WhenCalled_ReturnsNoContent()
    {
        // Arrange
        var controller = new OrdersController(this.mediator.Object, this.mapper.Object);
        var id = Guid.NewGuid();
        var cancellationToken = new CancellationToken();

        this.mediator
            .Setup(x => x.Send(It.IsAny<CompleteOrderCommand>(), cancellationToken))
            .Returns(Task.CompletedTask);

        // Act
        var result = await controller.CompleteOrder(id, cancellationToken);

        // Assert
        Assert.IsType<NoContentResult>(result);
        this.mediator.Verify(x => x.Send(It.IsAny<CompleteOrderCommand>(), cancellationToken), Times.Once);
    }

    [Fact]
    public async Task AddProductToOrderAsync_WhenCalled_ReturnsNoContent()
    {
        // Arrange
        var controller = new OrdersController(this.mediator.Object, this.mapper.Object);
        var id = Guid.NewGuid();
        var data = new CodeDesignPlus.Microservice.Api.Dtos.AddProductToOrderDto();
        var cancellationToken = new CancellationToken();

        this.mediator
            .Setup(x => x.Send(It.IsAny<AddProductToOrderCommand>(), cancellationToken))
            .Returns(Task.CompletedTask);

        // Act
        var result = await controller.AddProductToOrder(id, data, cancellationToken);

        // Assert
        Assert.IsType<NoContentResult>(result);
        this.mediator.Verify(x => x.Send(It.IsAny<AddProductToOrderCommand>(), cancellationToken), Times.Once);
    }

    [Fact]
    public async Task UpdateQuantityProductAsync_WhenCalled_ReturnsNoContent()
    {
        // Arrange
        var controller = new OrdersController(this.mediator.Object, this.mapper.Object);
        var id = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var data = new CodeDesignPlus.Microservice.Api.Dtos.UpdateQuantityProductDto();
        var cancellationToken = new CancellationToken();

        this.mediator
            .Setup(x => x.Send(It.IsAny<UpdateQuantityProductCommand>(), cancellationToken))
            .Returns(Task.CompletedTask);

        // Act
        var result = await controller.UpdateQuantityProduct(id, productId, data, cancellationToken);

        // Assert
        Assert.IsType<NoContentResult>(result);
        this.mediator.Verify(x => x.Send(It.IsAny<UpdateQuantityProductCommand>(), cancellationToken), Times.Once);
    }

    [Fact]
    public async Task RemoveProductFromOrderAsync_WhenCalled_ReturnsNoContent()
    {
        // Arrange
        var controller = new OrdersController(this.mediator.Object, this.mapper.Object);
        var id = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var cancellationToken = new CancellationToken();

        this.mediator
            .Setup(x => x.Send(It.IsAny<RemoveProductCommand>(), cancellationToken))
            .Returns(Task.CompletedTask);

        // Act
        var result = await controller.RemoveProduct(id, productId, cancellationToken);

        // Assert
        Assert.IsType<NoContentResult>(result);
        this.mediator.Verify(x => x.Send(It.IsAny<RemoveProductCommand>(), cancellationToken), Times.Once);
    }

}
