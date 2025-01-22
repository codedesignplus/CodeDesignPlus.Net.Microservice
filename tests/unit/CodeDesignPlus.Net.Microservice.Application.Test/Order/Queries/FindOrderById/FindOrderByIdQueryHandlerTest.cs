using CodeDesignPlus.Net.Cache.Abstractions;
using CodeDesignPlus.Net.Microservice.Application.Order.Queries.FindOrderById;

namespace CodeDesignPlus.Net.Microservice.Application.Test.Order.Queries.FindOrderById;

public class FindOrderByIdQueryHandlerTest
{
    private readonly IUserContext user;

    public FindOrderByIdQueryHandlerTest()
    {
        var userMock = new Mock<IUserContext>();

        userMock.Setup(x => x.IdUser).Returns(Guid.NewGuid());
        userMock.Setup(x => x.Tenant).Returns(Guid.NewGuid());

        this.user = userMock.Object;
    }

    [Fact]
    public async Task Handle_ValidId_ReturnsOrderDto()
    {
        // Arrange
        var orderRepository = new Mock<IOrderRepository>();
        var mapper = new Mock<IMapper>();
        var query = new FindOrderByIdQuery(Guid.NewGuid());
        var cancellationToken = new CancellationToken();
        var dtoExpected = new OrderDto();
        var cacheManagerMock = new Mock<ICacheManager>();

        cacheManagerMock.Setup(x => x.ExistsAsync(It.IsAny<string>())).ReturnsAsync(false);
        cacheManagerMock.Setup(x => x.SetAsync(It.IsAny<string>(), It.IsAny<OrderDto>(), It.IsAny<TimeSpan?>()));

        mapper.Setup(x => x.Map<OrderDto>(It.IsAny<object>())).Returns(dtoExpected);

        var handler = new FindOrderByIdQueryHandler(orderRepository.Object, mapper.Object, cacheManagerMock.Object, this.user);

        // Act
        var result = await handler.Handle(query, cancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OrderDto>(result);
        Assert.Equal(dtoExpected, result);
        orderRepository.Verify(x => x.FindAsync<OrderAggregate>(query.Id, this.user.Tenant, It.IsAny<CancellationToken>()), Times.Once);
        mapper.Verify(x => x.Map<OrderDto>(It.IsAny<object>()), Times.Once);
        cacheManagerMock.Verify(x => x.SetAsync(It.IsAny<string>(), It.IsAny<OrderDto>(), It.IsAny<TimeSpan?>()), Times.Once);
        cacheManagerMock.Verify(x => x.ExistsAsync(It.IsAny<string>()), Times.Once);
    }


    [Fact]
    public async Task Handle_Cache_ReturnsOrderDto()
    {
        // Arrange
        var orderRepository = new Mock<IOrderRepository>();
        var mapper = new Mock<IMapper>();
        var query = new FindOrderByIdQuery(Guid.NewGuid());
        var cancellationToken = new CancellationToken();
        var dtoExpected = new OrderDto();
        var cacheManagerMock = new Mock<ICacheManager>();

        cacheManagerMock.Setup(x => x.ExistsAsync(It.IsAny<string>())).ReturnsAsync(true);
        cacheManagerMock.Setup(x => x.GetAsync<OrderDto>(It.IsAny<string>())).ReturnsAsync(dtoExpected);

        mapper.Setup(x => x.Map<OrderDto>(It.IsAny<object>())).Returns(dtoExpected);

        var handler = new FindOrderByIdQueryHandler(orderRepository.Object, mapper.Object, cacheManagerMock.Object, this.user);

        // Act
        var result = await handler.Handle(query, cancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OrderDto>(result);
        Assert.Equal(dtoExpected, result);
        orderRepository.Verify(x => x.FindAsync<OrderAggregate>(query.Id, this.user.Tenant, It.IsAny<CancellationToken>()), Times.Never);
        mapper.Verify(x => x.Map<OrderDto>(It.IsAny<object>()), Times.Never);
        cacheManagerMock.Verify(x => x.GetAsync<OrderDto>(It.IsAny<string>()), Times.Once);
        cacheManagerMock.Verify(x => x.ExistsAsync(It.IsAny<string>()), Times.Once);
    }
}
