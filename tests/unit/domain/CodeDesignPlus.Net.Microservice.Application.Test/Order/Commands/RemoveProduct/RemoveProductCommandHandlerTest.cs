using System;
using CodeDesignPlus.Net.Exceptions;
using CodeDesignPlus.Net.Microservice.Application.Order.Commands.RemoveProduct;
using CodeDesignPlus.Net.Microservice.Domain.Entities;

namespace CodeDesignPlus.Net.Microservice.Application.Test.Order.Commands.RemoveProduct;

public class RemoveProductCommandHandlerTest
{

    [Fact]
    public async Task Handle_OrderNotFound_ThrowApplicaionException()
    {
        // Arrange
        var orderRepository = new Mock<IOrderRepository>();
        var message = new Mock<IMessage>();
        var handler = new RemoveProductCommandHandler(orderRepository.Object, message.Object);
        var request = new RemoveProductCommand(Guid.NewGuid(), Guid.NewGuid());

        orderRepository.Setup(x => x.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))!.ReturnsAsync((OrderAggregate)null!);

        // Act
        var exception = await Assert.ThrowsAsync<CodeDesignPlusException>(() => handler.Handle(request, CancellationToken.None));

        // Assert
        Assert.Equal(Errors.OrderNotFound.GetMessage(), exception.Message);
    }

    [Fact]
    public async Task Handle_Success()
    {
        // Arrange
        var orderRepository = new Mock<IOrderRepository>();
        var message = new Mock<IMessage>();
        var handler = new RemoveProductCommandHandler(orderRepository.Object, message.Object);
        var request = new RemoveProductCommand(Guid.NewGuid(), Guid.NewGuid());
        var order = OrderAggregate.Create(Guid.NewGuid(), Guid.NewGuid(), "Client", Guid.NewGuid());

        order.Products.Add(new ProductEntity()
        {
            Id = request.ProductId,
            Name = "Product",
            Price = 100,
            Quantity = 1
        });

        orderRepository.Setup(x => x.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))!.ReturnsAsync(order);

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert
        orderRepository.Verify(x => x.RemoveProductFromOrderAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        message.Verify(x => x.PublishAsync(It.IsAny<IReadOnlyList<IDomainEvent>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

}
