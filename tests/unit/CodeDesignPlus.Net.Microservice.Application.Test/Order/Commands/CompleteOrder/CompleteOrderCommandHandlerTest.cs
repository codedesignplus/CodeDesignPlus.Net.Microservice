using CodeDesignPlus.Net.Microservice.Application.Order.Commands.CompleteOrder;

namespace CodeDesignPlus.Net.Microservice.Application.Test.Order.Commands.CompleteOrder;

public class CompleteOrderCommandHandlerTest
{

    private readonly IUserContext user;

    public CompleteOrderCommandHandlerTest()
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
        var message = new Mock<IMessage>();

        var command = new CompleteOrderCommand(Guid.NewGuid());

        orderRepository.Setup(x => x.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))!.ReturnsAsync(default(OrderAggregate));

        var handler = new CompleteOrderCommandHandler(orderRepository.Object, this.user, message.Object);

        // Act
        var exception = await Assert.ThrowsAsync<Exceptions.CodeDesignPlusException>(() => handler.Handle(command, CancellationToken.None));

        // Assert
        orderRepository.Verify(x => x.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        orderRepository.Verify(x => x.AddProductToOrderAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<long?>(), It.IsAny<Guid?>(), It.IsAny<CancellationToken>()), Times.Never);
        message.Verify(x => x.PublishAsync(It.IsAny<IReadOnlyList<IDomainEvent>>(), It.IsAny<CancellationToken>()), Times.Never);

        Assert.Equal(Errors.OrderNotFound.GetCode(), exception.Code);
        Assert.Equal(Errors.OrderNotFound.GetMessage(), exception.Message);
    }

    [Fact]
    public async Task Handle_Success()
    {
        // Arrange
        var orderRepository = new Mock<IOrderRepository>();
        var message = new Mock<IMessage>();

        var order = OrderAggregate.Create(Guid.NewGuid(), Guid.NewGuid(), "client", this.user.Tenant, this.user.IdUser);

        var command = new CompleteOrderCommand(order.Id);

        orderRepository.Setup(x => x.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))!.ReturnsAsync(order);

        var handler = new CompleteOrderCommandHandler(orderRepository.Object, this.user, message.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert        
        Assert.NotNull(order.UpdatedAt);
        Assert.Equal(this.user.IdUser, order.UpdatedBy);
        orderRepository.Verify(x => x.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        orderRepository.Verify(x => x.CompleteOrderAsync(It.IsAny<Guid>(), It.IsAny<long?>(), It.IsAny<OrderStatus>(), It.IsAny<long?>(), It.IsAny<Guid?>(), It.IsAny<CancellationToken>()), Times.Once);
    }

}
