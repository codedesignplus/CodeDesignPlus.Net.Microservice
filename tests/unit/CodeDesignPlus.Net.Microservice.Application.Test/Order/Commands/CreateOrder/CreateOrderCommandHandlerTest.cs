using CodeDesignPlus.Net.Microservice.Application.Order.Commands.CreateOrder;

namespace CodeDesignPlus.Net.Microservice.Application.Test.Order.Commands.CreateOrder;

public class CreateOrderCommandHandlerTest
{
    private Mock<IUserContext> userMock;

    public CreateOrderCommandHandlerTest()
    {
        this.userMock = new Mock<IUserContext>();

        userMock.Setup(x => x.IdUser).Returns(Guid.NewGuid());
        userMock.Setup(x => x.Tenant).Returns(Guid.NewGuid());
    }

    [Fact]
    public async Task Handle_OrderAlreadyExists_ThrowApplicaionException()
    {
        // Arrange
        var orderRepository = new Mock<IOrderRepository>();
        var message = new Mock<IMessage>();

        var command = new CreateOrderCommand(Guid.NewGuid(), new ClientDto()
        {
            Id = Guid.NewGuid(),
            Name = "Client"
        });

        orderRepository
            .Setup(x => x.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))!
            .ReturnsAsync(OrderAggregate.Create(Guid.NewGuid(), Guid.NewGuid(), "Client", this.userMock.Object.Tenant, this.userMock.Object.IdUser));

        var handler = new CreateOrderCommandHandler(orderRepository.Object, this.userMock.Object, message.Object);

        // Act
        var exception = await Assert.ThrowsAsync<Exceptions.CodeDesignPlusException>(() => handler.Handle(command, CancellationToken.None));

        // Assert
        orderRepository.Verify(x => x.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        orderRepository.Verify(x => x.CreateOrderAsync(It.IsAny<OrderAggregate>(), It.IsAny<CancellationToken>()), Times.Never);
        message.Verify(x => x.PublishAsync(It.IsAny<IReadOnlyList<IDomainEvent>>(), It.IsAny<CancellationToken>()), Times.Never);

        Assert.Equal(Errors.OrderAlreadyExists.GetCode(), exception.Code);
        Assert.Equal(Errors.OrderAlreadyExists.GetMessage(), exception.Message);
    }

    [Fact]
    public async Task Handle_Success()
    {
        // Arrange
        var orderRepository = new Mock<IOrderRepository>();
        var message = new Mock<IMessage>();

        var command = new CreateOrderCommand(Guid.NewGuid(), new ClientDto()
        {
            Id = Guid.NewGuid(),
            Name = "Client"
        });

        var handler = new CreateOrderCommandHandler(orderRepository.Object, this.userMock.Object, message.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        orderRepository.Verify(x => x.CreateOrderAsync(It.IsAny<OrderAggregate>(), It.IsAny<CancellationToken>()), Times.Once);
        message.Verify(x => x.PublishAsync(It.IsAny<IReadOnlyList<IDomainEvent>>(), It.IsAny<CancellationToken>()), Times.Once);
        userMock.Verify(x => x.Tenant, Times.Once);
        userMock.Verify(x => x.IdUser, Times.Once);
    }

}
