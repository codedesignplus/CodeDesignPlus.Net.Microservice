using CodeDesignPlus.Net.Microservice.Application.Order.Commands.CreateOrder;
using CodeDesignPlus.Net.Microservice.Domain.ValueObjects;

namespace CodeDesignPlus.Net.Microservice.Application.Test.Order.Commands.CreateOrder;

public class CreateOrderCommandHandlerTest
{
    private readonly IUserContext user;

    public CreateOrderCommandHandlerTest()
    {
        var userMock = new Mock<IUserContext>();

        userMock.Setup(x => x.IdUser).Returns(Guid.NewGuid());
        userMock.Setup(x => x.Tenant).Returns(Guid.NewGuid());

        this.user = userMock.Object;
    }

    [Fact]
    public async Task Handle_OrderAlreadyExists_ThrowApplicaionException()
    {
        // Arrange
        var orderRepository = new Mock<IOrderRepository>();
        var pubsub = new Mock<IPubSub>();
        
        var clientValueObject = ClientValueObject.Create(Guid.NewGuid(), "Client", "1234567890", "CC");
        var addressValueObject = AddressValueObject.Create("Colombia", "Bogota", "Bogota", "Calle 123", 123456);

        var client = new ClientDto()
        {
            Id = Guid.NewGuid(),
            Name = "Client",
            Document = "1234567890",
            TypeDocument = "CC"
        };
        var address = new AddressDto(){
            Country = "Colombia",
            State = "Bogota",
            City = "Bogota",
            Address = "Calle 123",
            CodePostal = 123456
        };

        var command = new CreateOrderCommand(Guid.NewGuid(), client, address);

        orderRepository
            .Setup(x => x.ExistsAsync<OrderAggregate>(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var handler = new CreateOrderCommandHandler(orderRepository.Object, this.user, pubsub.Object);

        // Act
        var exception = await Assert.ThrowsAsync<Exceptions.CodeDesignPlusException>(() => handler.Handle(command, CancellationToken.None));

        // Assert
        orderRepository.Verify(x => x.ExistsAsync<OrderAggregate>(command.Id, this.user.Tenant, It.IsAny<CancellationToken>()), Times.Once);
        orderRepository.Verify(x => x.CreateAsync(It.IsAny<OrderAggregate>(), It.IsAny<CancellationToken>()), Times.Never);
        pubsub.Verify(x => x.PublishAsync(It.IsAny<IReadOnlyList<IDomainEvent>>(), It.IsAny<CancellationToken>()), Times.Never);

        Assert.Equal(Errors.OrderAlreadyExists.GetCode(), exception.Code);
        Assert.Equal(Errors.OrderAlreadyExists.GetMessage(), exception.Message);
    }

    [Fact]
    public async Task Handle_Success()
    {
        // Arrange
        var orderRepository = new Mock<IOrderRepository>();
        var pubsub = new Mock<IPubSub>();

        var client = new ClientDto()
        {
            Id = Guid.NewGuid(),
            Name = "Client",
            Document = "1234567890",
            TypeDocument = "CC"
        };
        var address = new AddressDto(){
            Country = "Colombia",
            State = "Bogota",
            City = "Bogota",
            Address = "Calle 123",
            CodePostal = 123456
        };


        var command = new CreateOrderCommand(Guid.NewGuid(), client, address);

        var handler = new CreateOrderCommandHandler(orderRepository.Object, this.user, pubsub.Object);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        orderRepository.Verify(x => x.CreateAsync(It.IsAny<OrderAggregate>(), It.IsAny<CancellationToken>()), Times.Once);
        pubsub.Verify(x => x.PublishAsync(It.IsAny<IReadOnlyList<IDomainEvent>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

}
