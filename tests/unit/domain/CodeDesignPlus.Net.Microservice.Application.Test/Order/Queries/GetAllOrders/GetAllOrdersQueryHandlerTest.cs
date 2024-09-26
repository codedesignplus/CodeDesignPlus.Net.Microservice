using System;
using System.Linq.Expressions;
using CodeDesignPlus.Net.Microservice.Application.Order.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Application.Order.Queries.GetAllOrders;
using CodeDesignPlus.Net.Microservice.Domain;
using CodeDesignPlus.Net.Microservice.Domain.Repositories;
using MapsterMapper;
using Moq;
using C = CodeDesignPlus.Net.Core.Abstractions.Models.Criteria;

namespace CodeDesignPlus.Net.Microservice.Application.Test.Order.Queries.GetAllOrders;

public class GetAllOrdersQueryHandlerTest
{

    [Fact]
    public async Task Handle_Success()
    {
        // Arrange
        var criteria = new C.Criteria();
        var query = new GetAllOrdersQuery(criteria);
        var orders = new List<OrderDto>
        {
            new() {
                Id = Guid.NewGuid(),
                Client = new ClientDto(){
                    Name = "Client 1",
                },
                CompletedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                CreatedBy = Guid.NewGuid(),
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                IsActive = true,
                Products = [
                    new(){
                        Id = Guid.NewGuid(),
                        Name = "Product 1",
                        Price = 100,
                        Quantity = 1
                    }
                ],

            },
            new() {
                Id = Guid.NewGuid(),
                Client = new ClientDto(){
                    Name = "Client 2",
                },
                CompletedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                CreatedBy = Guid.NewGuid(),
                CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                IsActive = true,
                Products = [
                    new(){
                        Id = Guid.NewGuid(),
                        Name = "Product 2",
                        Price = 200,
                        Quantity = 2
                    }
                ],

            }
        };

        var orderRepositoryMock = new Mock<IOrderRepository>();
        var mapperMock = new Mock<IMapper>();
        var queryHandler = new GetAllOrdersQueryHandler(orderRepositoryMock.Object, mapperMock.Object);

        mapperMock.Setup(x => x.Map<List<OrderDto>>(It.IsAny<List<OrderAggregate>>())).Returns(orders);

        // Act
        var result = await queryHandler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(orders.Count, result.Count);

        orderRepositoryMock.Verify(x => x.MatchingAsync<OrderAggregate>(criteria, It.IsAny<CancellationToken>()), Times.Once);
        mapperMock.Verify(x => x.Map<List<OrderDto>>(It.IsAny<List<OrderAggregate>>()), Times.Once);
    }

}
