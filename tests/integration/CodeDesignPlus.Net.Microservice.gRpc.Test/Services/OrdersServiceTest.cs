using CodeDesignPlus.Net.Microservice.Domain;
using CodeDesignPlus.Net.Microservice.Domain.Repositories;
using CodeDesignPlus.Net.Microservice.gRpc.Test.Helpers.Server;
using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;

namespace CodeDesignPlus.Net.Microservice.gRpc.Test.Services;

[Collection("Server Collection")]
public class OrdersServiceTest(TestServer<Program> server) : TestBase(server)
{

    [Fact]
    public async Task GetOrder_BidirectionalStreaming_ValidId_ReturnsOrder()
    {
        bool isInvoked = false;
        var orderClient = new Orders.OrdersClient(_channel);

        var orderExpected = OrderAggregate.Create(Guid.NewGuid(), Guid.NewGuid(), "CodeDesignPlus", Guid.NewGuid(), Guid.NewGuid());

        var repository = _services.GetRequiredService<IOrderRepository>();

        await repository.CreateAsync(orderExpected, CancellationToken.None);

        using var streamingCall = orderClient.GetOrder();

        _ = Task.Run(async () =>
        {
            await foreach (var response in streamingCall.ResponseStream.ReadAllAsync())
            {
                Assert.NotNull(response);
                Assert.Equal(orderExpected.Id.ToString(), response.Order.Id);
                Assert.Equal(orderExpected.Client.Id.ToString(), response.Order.Client.Id);
                Assert.Equal(orderExpected.Client.Name, response.Order.Client.Name);
                Assert.Equal(orderExpected.CreatedBy.ToString(), response.Order.CreatedBy);
                Assert.True(response.Order.CreatedAt > 0);

                isInvoked = true;
            }
        });

        await streamingCall.RequestStream.WriteAsync(new GetOrderRequest
        {
            Id = orderExpected.Id.ToString()
        });

        await Task.Delay(1000);

        await streamingCall.RequestStream.CompleteAsync();

        Assert.True(isInvoked);
    }

}
