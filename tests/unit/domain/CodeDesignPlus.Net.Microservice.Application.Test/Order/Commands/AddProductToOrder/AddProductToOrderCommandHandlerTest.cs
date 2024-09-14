
using CodeDesignPlus.Net.Core.Abstractions;
using CodeDesignPlus.Net.Exceptions.Extensions;
using CodeDesignPlus.Net.Microservice.Application.Order.Commands.AddProductToOrder;
using CodeDesignPlus.Net.Microservice.Domain;
using CodeDesignPlus.Net.Microservice.Domain.Repositories;
using CodeDesignPlus.Net.PubSub.Abstractions;
using Moq;

namespace CodeDesignPlus.Net.Microservice.Application.Test.Order.Commands.AddProductToOrder;

public class AddProductToOrderCommandHandlerTest
{
    [Fact]
    public async Task Handle_OrderNotFound_ThrowApplicaionException()
    {
        // Arrange
        var orderRepository = new Mock<IOrderRepository>();
        var message = new Mock<IMessage>();

        var command = new AddProductToOrderCommand
        (
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Product 1",
            "Description Product 1",
            100,
            1
        );

        orderRepository.Setup(x => x.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))!.ReturnsAsync(default(OrderAggregate));

        var handler = new AddProductToOrderCommandHandler(orderRepository.Object, message.Object);

        // Act
        var exception = await Assert.ThrowsAsync<Exceptions.CodeDesignPlusException>(() => handler.Handle(command, CancellationToken.None));

        // Assert
        orderRepository.Verify(x => x.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        orderRepository.Verify(x => x.AddProductToOrderAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
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

        var order = OrderAggregate.Create(Guid.NewGuid(), Guid.NewGuid(), "client", Guid.NewGuid());

        var command = new AddProductToOrderCommand
        (
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Product 1",
            "Description Product 1",
            100,
            1
        );

        orderRepository.Setup(x => x.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())).ReturnsAsync(order);

        var handler = new AddProductToOrderCommandHandler(orderRepository.Object, message.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        orderRepository.Verify(x => x.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        orderRepository.Verify(x => x.AddProductToOrderAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        message.Verify(x => x.PublishAsync(It.IsAny<IReadOnlyList<IDomainEvent>>(), It.IsAny<CancellationToken>()), Times.Once);

        Assert.NotEmpty(order.Products);
        Assert.Contains(order.Products, 
            x => 
            x.Name == command.Name && 
            x.Description == command.Description && 
            x.Price == command.Price && 
            x.Quantity == command.Quantity
        );
    }

}
