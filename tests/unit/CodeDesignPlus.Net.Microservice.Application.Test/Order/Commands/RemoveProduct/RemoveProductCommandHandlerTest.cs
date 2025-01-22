using CodeDesignPlus.Net.Microservice.Application.Order.Commands.RemoveProduct;
using CodeDesignPlus.Net.Microservice.Domain.ValueObjects;

namespace CodeDesignPlus.Net.Microservice.Application.Test.Order.Commands.RemoveProduct;

public class RemoveProductCommandHandlerTest
{
    private readonly Guid idUser = Guid.NewGuid();
    private readonly Guid tenant = Guid.NewGuid();
    private readonly IUserContext user;

    public RemoveProductCommandHandlerTest()
    {
        var userMock = new Mock<IUserContext>();

        userMock.Setup(x => x.IdUser).Returns(idUser);
        userMock.Setup(x => x.Tenant).Returns(tenant);

        this.user = userMock.Object;
    }

    [Fact]
    public async Task Handle_OrderNotFound_ThrowApplicaionException()
    {
        // Arrange
        var orderRepository = new Mock<IOrderRepository>();
        var pubsub = new Mock<IPubSub>();
        var handler = new RemoveProductCommandHandler(orderRepository.Object, user, pubsub.Object);
        var request = new RemoveProductCommand(Guid.NewGuid(), Guid.NewGuid());

        orderRepository.Setup(x => x.FindAsync<OrderAggregate>(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))!.ReturnsAsync((OrderAggregate)null!);

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
        var pubsub = new Mock<IPubSub>();
        var handler = new RemoveProductCommandHandler(orderRepository.Object, user, pubsub.Object);
        var request = new RemoveProductCommand(Guid.NewGuid(), Guid.NewGuid());
        var clientValueObject = ClientValueObject.Create(Guid.NewGuid(), "Client", "1234567890", "CC");
        var addressValueObject = AddressValueObject.Create("Colombia", "Bogota", "Bogota", "Calle 123", 123456);

        var order = OrderAggregate.Create(Guid.NewGuid(), clientValueObject, addressValueObject, this.user.Tenant, this.user.IdUser);

        order.Products.Add(new ProductEntity()
        {
            Id = request.ProductId,
            Name = "Product",
            Price = 100,
            Quantity = 1
        });

        orderRepository.Setup(x => x.FindAsync<OrderAggregate>(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))!.ReturnsAsync(order);

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert        
        Assert.NotNull(order.UpdatedAt);
        Assert.Equal(this.user.IdUser, order.UpdatedBy);
        orderRepository.Verify(x => x.RemoveProductFromOrderAsync(It.IsAny<RemoveProductFromOrderParams>(), this.user.Tenant, It.IsAny<CancellationToken>()), Times.Once);
        pubsub.Verify(x => x.PublishAsync(It.IsAny<IReadOnlyList<IDomainEvent>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

}
