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
        await orderRepository.AddProductToOrderAsync(idOrder, idProduct, name, description, price, quantity, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), Guid.NewGuid(), cancellationToken);

        // Assert
        collectionMock.Verify(x => x.UpdateOneAsync(It.IsAny<FilterDefinition<OrderAggregate>>(), It.IsAny<UpdateDefinition<OrderAggregate>>(), It.IsAny<UpdateOptions>(), cancellationToken), Times.Once);
    }

    [Fact]
    public async Task CancelOrderAsync_Should_Cancel_Order()
    {
        // Arrange
        var idOrder = Guid.NewGuid();
        var reason = "Reason for cancellation";
        var cancellationToken = CancellationToken.None;
        var cancelledAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        collectionMock
            .Setup(x => x.UpdateOneAsync(It.IsAny<FilterDefinition<OrderAggregate>>(), It.IsAny<UpdateDefinition<OrderAggregate>>(), It.IsAny<UpdateOptions>(), cancellationToken))
            .ReturnsAsync(It.IsAny<UpdateResult>());

        // Act
        await orderRepository.CancelOrderAsync(idOrder, OrderStatus.Cancelled, reason, cancelledAt, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), Guid.NewGuid(), cancellationToken);

        // Assert
        collectionMock.Verify(x => x.UpdateOneAsync(It.IsAny<FilterDefinition<OrderAggregate>>(), It.IsAny<UpdateDefinition<OrderAggregate>>(), It.IsAny<UpdateOptions>(), cancellationToken), Times.Once);
    }

    [Fact]
    public async Task CompleteOrderAsync_Should_Complete_Order()
    {
        // Arrange
        var idOrder = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;
        var completedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        collectionMock
            .Setup(x => x.UpdateOneAsync(It.IsAny<FilterDefinition<OrderAggregate>>(), It.IsAny<UpdateDefinition<OrderAggregate>>(), It.IsAny<UpdateOptions>(), cancellationToken))
            .ReturnsAsync(It.IsAny<UpdateResult>());

        // Act
        await orderRepository.CompleteOrderAsync(idOrder, completedAt, OrderStatus.Completed, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), Guid.NewGuid(), cancellationToken);

        // Assert
        collectionMock.Verify(x => x.UpdateOneAsync(It.IsAny<FilterDefinition<OrderAggregate>>(), It.IsAny<UpdateDefinition<OrderAggregate>>(), It.IsAny<UpdateOptions>(), cancellationToken), Times.Once);
    }

    [Fact]
    public async Task CreateOrderAsync_Should_Create_Order()
    {
        // Arrange
        var order = OrderAggregate.Create(Guid.NewGuid(), Guid.NewGuid(), "Order Name", Guid.NewGuid(), Guid.NewGuid());
        var cancellationToken = CancellationToken.None;

        collectionMock
            .Setup(x => x.InsertOneAsync(It.IsAny<OrderAggregate>(), It.IsAny<InsertOneOptions>(), cancellationToken));

        // Act
        await orderRepository.CreateOrderAsync(order, cancellationToken);

        // Assert
        collectionMock.Verify(x => x.InsertOneAsync(It.IsAny<OrderAggregate>(), It.IsAny<InsertOneOptions>(), cancellationToken), Times.Once);
    }

    [Fact]
    public async Task FindAsync_Should_Find_Order()
    {
        // Arrange
        var id = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;

        collectionMock
            .Setup(x => x.FindAsync(It.IsAny<FilterDefinition<OrderAggregate>>(), It.IsAny<FindOptions<OrderAggregate, OrderAggregate>>(), cancellationToken))
            .ReturnsAsync(new Mock<IAsyncCursor<OrderAggregate>>().Object);

        // Act
        await orderRepository.FindAsync(id, cancellationToken);

        // Assert
        collectionMock.Verify(x => x.FindAsync(It.IsAny<FilterDefinition<OrderAggregate>>(), It.IsAny<FindOptions<OrderAggregate, OrderAggregate>>(), cancellationToken), Times.Once);
    }

    [Fact]
    public async Task GetAllOrdersAsync_Should_Get_All_Orders()
    {
        // Arrange
        var cancellationToken = CancellationToken.None;

        collectionMock
            .Setup(x => x.FindAsync(It.IsAny<FilterDefinition<OrderAggregate>>(), It.IsAny<FindOptions<OrderAggregate, OrderAggregate>>(), cancellationToken))
            .ReturnsAsync(new Mock<IAsyncCursor<OrderAggregate>>().Object);

        // Act
        await orderRepository.GetAllOrdersAsync(cancellationToken);

        // Assert
        collectionMock.Verify(x => x.FindAsync(It.IsAny<FilterDefinition<OrderAggregate>>(), It.IsAny<FindOptions<OrderAggregate, OrderAggregate>>(), cancellationToken), Times.Once);
    }

    [Fact]
    public async Task RemoveProductFromOrderAsync_Should_Remove_Product_From_Order()
    {
        // Arrange
        var idOrder = Guid.NewGuid();
        var idProduct = Guid.NewGuid();
        var cancellationToken = CancellationToken.None;

        collectionMock
            .Setup(x => x.UpdateOneAsync(It.IsAny<FilterDefinition<OrderAggregate>>(), It.IsAny<UpdateDefinition<OrderAggregate>>(), It.IsAny<UpdateOptions>(), cancellationToken))
            .ReturnsAsync(It.IsAny<UpdateResult>());

        // Act
        await orderRepository.RemoveProductFromOrderAsync(idOrder, idProduct, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), Guid.NewGuid(), cancellationToken);

        // Assert
        collectionMock.Verify(x => x.UpdateOneAsync(It.IsAny<FilterDefinition<OrderAggregate>>(), It.IsAny<UpdateDefinition<OrderAggregate>>(), It.IsAny<UpdateOptions>(), cancellationToken), Times.Once);
    }

    [Fact]
    public async Task UpdateOrderAsync_Should_Update_Order()
    {
        // Arrange
        var order = OrderAggregate.Create(Guid.NewGuid(), Guid.NewGuid(), "Order Name", Guid.NewGuid(), Guid.NewGuid());
        var cancellationToken = CancellationToken.None;

        collectionMock
            .Setup(x => x.ReplaceOneAsync(It.IsAny<FilterDefinition<OrderAggregate>>(), It.IsAny<OrderAggregate>(), It.IsAny<ReplaceOptions>(), cancellationToken))
            .ReturnsAsync(It.IsAny<ReplaceOneResult>());

        // Act
        await orderRepository.UpdateOrderAsync(order, cancellationToken);

        // Assert
        collectionMock.Verify(x => x.ReplaceOneAsync(It.IsAny<FilterDefinition<OrderAggregate>>(), It.IsAny<OrderAggregate>(), It.IsAny<ReplaceOptions>(), cancellationToken), Times.Once);
    }

    [Fact]
    public async Task UpdateQuantityProductAsync_Should_Update_Quantity_Product()
    {
        // Arrange
        var idOrder = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var newQuantity = 10;
        var cancellationToken = CancellationToken.None;

        collectionMock
            .Setup(x => x.UpdateOneAsync(It.IsAny<FilterDefinition<OrderAggregate>>(), It.IsAny<UpdateDefinition<OrderAggregate>>(), It.IsAny<UpdateOptions>(), cancellationToken))
            .ReturnsAsync(It.IsAny<UpdateResult>());

        // Act
        await orderRepository.UpdateQuantityProductAsync(idOrder, productId, newQuantity, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(), Guid.NewGuid(), cancellationToken);

        // Assert
        collectionMock.Verify(x => x.UpdateOneAsync(It.IsAny<FilterDefinition<OrderAggregate>>(), It.IsAny<UpdateDefinition<OrderAggregate>>(), It.IsAny<UpdateOptions>(), cancellationToken), Times.Once);
    }
}
