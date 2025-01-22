using CodeDesignPlus.Net.Microservice.Domain.ValueObjects;

namespace CodeDesignPlus.Net.Microservice.Infrastructure.Test.Repositories;

public class OrderRepositoryTest
{
    private readonly ServiceProvider serviceProvider;
    private readonly Mock<IMongoClient> mongoClientMock;
    private readonly Mock<IMongoDatabase> mongoDatabaseMock;
    private readonly IOptions<MongoOptions> mongoOptions;
    private readonly Mock<ILogger<RepositoryBase>> loggerMock;
    private readonly Mock<IMongoCollection<OrderAggregate>> collectionMock;
    private readonly OrderRepository orderRepository;

    public OrderRepositoryTest()
    {
        mongoClientMock = new Mock<IMongoClient>();
        mongoDatabaseMock = new Mock<IMongoDatabase>();
        this.mongoOptions = Options.Create(new MongoOptions());
        loggerMock = new Mock<ILogger<RepositoryBase>>();
        collectionMock = new Mock<IMongoCollection<OrderAggregate>>();

        mongoDatabaseMock.Setup(x => x.GetCollection<OrderAggregate>(It.IsAny<string>(), It.IsAny<MongoCollectionSettings>()))
            .Returns(collectionMock.Object);

        mongoClientMock.Setup(x => x.GetDatabase(It.IsAny<string>(), It.IsAny<MongoDatabaseSettings>()))
            .Returns(mongoDatabaseMock.Object);

        var serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton(mongoClientMock.Object);
        serviceCollection.AddSingleton(this.mongoOptions);
        serviceCollection.AddSingleton(loggerMock.Object);

        serviceProvider = serviceCollection.BuildServiceProvider();

        var mongoOptions2 = serviceProvider.GetService<IOptions<MongoOptions>>();
        var logger = serviceProvider.GetService<ILogger<RepositoryBase>>();

        orderRepository = new OrderRepository(
            serviceProvider,
            mongoOptions2!,
            logger!
        );
    }

    [Fact]
    public async Task AddProductToOrderAsync_Should_Add_Product_To_Order()
    {
        // Arrange
        var idOrder = Guid.NewGuid();
        var tenant = Guid.NewGuid();
        var idProduct = Guid.NewGuid();
        var name = "Product Name";
        var description = "Product Description";
        var price = 100;
        var quantity = 1;
        var cancellationToken = CancellationToken.None;

        collectionMock
            .Setup(x => x.UpdateOneAsync(It.IsAny<FilterDefinition<OrderAggregate>>(), It.IsAny<UpdateDefinition<OrderAggregate>>(), It.IsAny<UpdateOptions>(), cancellationToken))
            .ReturnsAsync(It.IsAny<UpdateResult>());

        // Act
        await orderRepository.AddProductToOrderAsync(idOrder, tenant, new AddProductToOrderParams()
        {
            Id = idProduct,
            Name = name,
            Description = description,
            Price = price,
            Quantity = quantity,
            UpdatedAt = SystemClock.Instance.GetCurrentInstant(),
            UpdateBy = Guid.NewGuid()
        }, cancellationToken);

        // Assert
        collectionMock.Verify(x => x.UpdateOneAsync(It.IsAny<FilterDefinition<OrderAggregate>>(), It.IsAny<UpdateDefinition<OrderAggregate>>(), It.IsAny<UpdateOptions>(), cancellationToken), Times.Once);
    }

    [Fact]
    public async Task CancelOrderAsync_Should_Cancel_Order()
    {
        // Arrange
        var idOrder = Guid.NewGuid();
        var tenant = Guid.NewGuid();
        var reason = "Reason for cancellation";
        var cancellationToken = CancellationToken.None;
        var cancelledAt = SystemClock.Instance.GetCurrentInstant();

        collectionMock
            .Setup(x => x.UpdateOneAsync(It.IsAny<FilterDefinition<OrderAggregate>>(), It.IsAny<UpdateDefinition<OrderAggregate>>(), It.IsAny<UpdateOptions>(), cancellationToken))
            .ReturnsAsync(It.IsAny<UpdateResult>());

        // Act
        await orderRepository.CancelOrderAsync(new CancelOrderParams()
        {
            Id = idOrder,
            OrderStatus = OrderStatus.Cancelled,
            Reason = reason,
            CancelledAt = cancelledAt,
            UpdatedAt = SystemClock.Instance.GetCurrentInstant(),
            UpdateBy = Guid.NewGuid()
        }, tenant, cancellationToken);

        // Assert
        collectionMock.Verify(x => x.UpdateOneAsync(It.IsAny<FilterDefinition<OrderAggregate>>(), It.IsAny<UpdateDefinition<OrderAggregate>>(), It.IsAny<UpdateOptions>(), cancellationToken), Times.Once);
    }

    [Fact]
    public async Task CompleteOrderAsync_Should_Complete_Order()
    {
        // Arrange
        var idOrder = Guid.NewGuid();
        var tenant = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;
        var completedAt = SystemClock.Instance.GetCurrentInstant();

        collectionMock
            .Setup(x => x.UpdateOneAsync(It.IsAny<FilterDefinition<OrderAggregate>>(), It.IsAny<UpdateDefinition<OrderAggregate>>(), It.IsAny<UpdateOptions>(), cancellationToken))
            .ReturnsAsync(It.IsAny<UpdateResult>());

        // Act
        await orderRepository.CompleteOrderAsync(new CompleteOrderParams()
        {
            Id = idOrder,
            CompletedAt = completedAt,
            OrderStatus = OrderStatus.Completed,
            UpdatedAt = SystemClock.Instance.GetCurrentInstant(),
            UpdateBy = Guid.NewGuid()
        }, tenant, cancellationToken);

        // Assert
        collectionMock.Verify(x => x.UpdateOneAsync(It.IsAny<FilterDefinition<OrderAggregate>>(), It.IsAny<UpdateDefinition<OrderAggregate>>(), It.IsAny<UpdateOptions>(), cancellationToken), Times.Once);
    }

    [Fact]
    public async Task RemoveProductFromOrderAsync_Should_Remove_Product_From_Order()
    {
        // Arrange
        var idOrder = Guid.NewGuid();
        var tenant = Guid.NewGuid();
        var idProduct = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;

        collectionMock
            .Setup(x => x.UpdateOneAsync(It.IsAny<FilterDefinition<OrderAggregate>>(), It.IsAny<UpdateDefinition<OrderAggregate>>(), It.IsAny<UpdateOptions>(), cancellationToken))
            .ReturnsAsync(It.IsAny<UpdateResult>());

        // Act
        await orderRepository.RemoveProductFromOrderAsync(new RemoveProductFromOrderParams()
        {
            Id = idOrder,
            IdProduct = idProduct,
            UpdatedAt = SystemClock.Instance.GetCurrentInstant(),
            UpdateBy = Guid.NewGuid()
        }, tenant, cancellationToken);

        // Assert
        collectionMock.Verify(x => x.UpdateOneAsync(It.IsAny<FilterDefinition<OrderAggregate>>(), It.IsAny<UpdateDefinition<OrderAggregate>>(), It.IsAny<UpdateOptions>(), cancellationToken), Times.Once);
    }

    [Fact]
    public async Task UpdateQuantityProductAsync_Should_Update_Quantity_Product()
    {
        // Arrange
        var idOrder = Guid.NewGuid();
        var tenant = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var newQuantity = 10;
        var cancellationToken = CancellationToken.None;

        collectionMock
            .Setup(x => x.UpdateOneAsync(It.IsAny<FilterDefinition<OrderAggregate>>(), It.IsAny<UpdateDefinition<OrderAggregate>>(), It.IsAny<UpdateOptions>(), cancellationToken))
            .ReturnsAsync(It.IsAny<UpdateResult>());

        // Act
        await orderRepository.UpdateQuantityProductAsync(idOrder, tenant, new UpdateQuantityProductParams()
        {
            Id = productId,
            NewQuantity = newQuantity,
            UpdatedAt = SystemClock.Instance.GetCurrentInstant(),
            UpdateBy = Guid.NewGuid()
        }, cancellationToken);

        // Assert
        collectionMock.Verify(x => x.UpdateOneAsync(It.IsAny<FilterDefinition<OrderAggregate>>(), It.IsAny<UpdateDefinition<OrderAggregate>>(), It.IsAny<UpdateOptions>(), cancellationToken), Times.Once);
    }
}
