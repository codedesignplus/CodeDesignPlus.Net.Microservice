using CodeDesignPlus.Net.Microservice.Application.Order.Commands.CreateOrder;

namespace CodeDesignPlus.Net.Microservice.Application.Test.Order.Commands.CreateOrder;

public class CreateOrderCommandHandlerTest
{
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
            .ReturnsAsync(OrderAggregate.Create(Guid.NewGuid(), Guid.NewGuid(), "Client", Guid.NewGuid()));

        var handler = new CreateOrderCommandHandler(orderRepository.Object, message.Object);

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

        var handler = new CreateOrderCommandHandler(orderRepository.Object, message.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        orderRepository.Verify(x => x.CreateOrderAsync(It.IsAny<OrderAggregate>(), It.IsAny<CancellationToken>()), Times.Once);
        message.Verify(x => x.PublishAsync(It.IsAny<IReadOnlyList<IDomainEvent>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

}
