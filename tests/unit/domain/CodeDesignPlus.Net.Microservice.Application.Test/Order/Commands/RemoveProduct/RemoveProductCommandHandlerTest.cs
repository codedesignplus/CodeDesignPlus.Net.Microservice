using CodeDesignPlus.Net.Exceptions;
using CodeDesignPlus.Net.Microservice.Application.Order.Commands.RemoveProduct;
using CodeDesignPlus.Net.Microservice.Domain.Entities;
using CodeDesignPlus.Net.Security.Abstractions;

namespace CodeDesignPlus.Net.Microservice.Application.Test.Order.Commands.RemoveProduct;

public class RemoveProductCommandHandlerTest
{
    private readonly IUserContext user;

    public RemoveProductCommandHandlerTest()
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
        var handler = new RemoveProductCommandHandler(orderRepository.Object, user, message.Object);
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
        var handler = new RemoveProductCommandHandler(orderRepository.Object, user, message.Object);
        var request = new RemoveProductCommand(Guid.NewGuid(), Guid.NewGuid());
        var order = OrderAggregate.Create(Guid.NewGuid(), Guid.NewGuid(), "Client", this.user.Tenant, this.user.IdUser);

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
        Assert.NotNull(order.UpdatedAt);
        Assert.Equal(this.user.IdUser, order.UpdatedBy);
        orderRepository.Verify(x => x.RemoveProductFromOrderAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<long?>(), It.IsAny<Guid?>(), It.IsAny<CancellationToken>()), Times.Once);
        message.Verify(x => x.PublishAsync(It.IsAny<IReadOnlyList<IDomainEvent>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

}
