using System;
using CodeDesignPlus.Net.Microservice.Application.Order.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Application.Order.Queries.FindOrderById;
using CodeDesignPlus.Net.Microservice.Domain.Repositories;
using MapsterMapper;
using Moq;

namespace CodeDesignPlus.Net.Microservice.Application.Test.Order.Queries.FindOrderById;

public class FindOrderByIdQueryHandlerTest
{
    [Fact]
    public async Task Handle_ValidId_ReturnsOrderDto()
    {
        // Arrange
        var orderRepository = new Mock<IOrderRepository>();
        var mapper = new Mock<IMapper>();
        var query = new FindOrderByIdQuery(Guid.NewGuid());
        var cancellationToken = new CancellationToken();
        var dtoExpected = new OrderDto();

        mapper.Setup(x => x.Map<OrderDto>(It.IsAny<object>())).Returns(dtoExpected);

        var handler = new FindOrderByIdQueryHandler(orderRepository.Object, mapper.Object);

        // Act
        var result = await handler.Handle(query, cancellationToken);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OrderDto>(result);
        Assert.Equal(dtoExpected, result);
        orderRepository.Verify(x => x.FindAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
        mapper.Verify(x => x.Map<OrderDto>(It.IsAny<object>()), Times.Once);
    }
}
