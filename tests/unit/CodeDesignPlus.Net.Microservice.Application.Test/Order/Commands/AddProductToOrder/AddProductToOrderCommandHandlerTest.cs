using CodeDesignPlus.Net.Microservice.Application.Order.Commands.AddProductToOrder;
using CodeDesignPlus.Net.Microservice.Domain.ValueObjects;

namespace CodeDesignPlus.Net.Microservice.Application.Test.Order.Commands.AddProductToOrder;

public class AddProductToOrderCommandHandlerTest
{
    private readonly IUserContext user;

    public AddProductToOrderCommandHandlerTest()
    {
        var userMock = new Mock<IUserContext>();

        userMock.Setup(x => x.IdUser).Returns(Guid.NewGuid());
        userMock.Setup(x => x.Tenant).Returns(Guid.NewGuid());

        this.user = userMock.Object;
    }

    [Fact]
    public async Task Handle_OrderNotFound_ThrowApplicaionException()
    {
        // Arrange
        var orderRepository = new Mock<IOrderRepository>();
        var pubsub = new Mock<IPubSub>();

        var command = new AddProductToOrderCommand
        (
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Product 1",
            "Description Product 1",
            100,
            1
        );

        orderRepository.Setup(x => x.FindAsync<OrderAggregate>(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))!.ReturnsAsync(default(OrderAggregate));

        var handler = new AddProductToOrderCommandHandler(orderRepository.Object, this.user, pubsub.Object);

        // Act
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(command, CancellationToken.None));

        // Assert
        orderRepository.Verify(x => x.FindAsync<OrderAggregate>(command.Id, this.user.Tenant, It.IsAny<CancellationToken>()), Times.Once);
        orderRepository.Verify(x => x.AddProductToOrderAsync(command.Id, this.user.Tenant, It.IsAny<AddProductToOrderParams>(), It.IsAny<CancellationToken>()), Times.Never);
        pubsub.Verify(x => x.PublishAsync(It.IsAny<IReadOnlyList<IDomainEvent>>(), It.IsAny<CancellationToken>()), Times.Never);

        Assert.Equal(Errors.OrderNotFound.GetCode(), exception.Code);
        Assert.Equal(Errors.OrderNotFound.GetMessage(), exception.Message);
    }

    [Fact]
    public async Task Handle_Success()
    {
        // Arrange
        var orderRepository = new Mock<IOrderRepository>();
        var pubsub = new Mock<IPubSub>();
        var client = ClientValueObject.Create(Guid.NewGuid(), "client", "1234567890", "CC");
        var address = AddressValueObject.Create("Colombia", "Bogota", "Bogota", "Calle 123", 123456);

        var order = OrderAggregate.Create(Guid.NewGuid(), client, address, this.user.Tenant, this.user.IdUser);

        var command = new AddProductToOrderCommand
        (
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Product 1",
            "Description Product 1",
            100,
            1
        );

        orderRepository.Setup(x => x.FindAsync<OrderAggregate>(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(order);

        var handler = new AddProductToOrderCommandHandler(orderRepository.Object, this.user, pubsub.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        orderRepository.Verify(x => x.FindAsync<OrderAggregate>(command.Id, this.user.Tenant, It.IsAny<CancellationToken>()), Times.Once);
        orderRepository.Verify(x => x.AddProductToOrderAsync(order.Id, this.user.Tenant, It.IsAny<AddProductToOrderParams>(), It.IsAny<CancellationToken>()), Times.Once);
        pubsub.Verify(x => x.PublishAsync(It.IsAny<IReadOnlyList<IDomainEvent>>(), It.IsAny<CancellationToken>()), Times.Once);

        Assert.NotEmpty(order.Products);
        Assert.NotNull(order.UpdatedAt);
        Assert.Equal(this.user.IdUser, order.UpdatedBy);
        Assert.Contains(order.Products,
            x =>
            x.Name == command.Name &&
            x.Description == command.Description &&
            x.Price == command.Price &&
            x.Quantity == command.Quantity
        );
    }

}
