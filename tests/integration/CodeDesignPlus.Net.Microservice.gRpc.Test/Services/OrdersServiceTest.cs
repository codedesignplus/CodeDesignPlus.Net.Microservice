using Microsoft.Extensions.Configuration;

namespace CodeDesignPlus.Net.Microservice.gRpc.Test.Services;

public class OrdersServiceTest : ServerBase<Program>, IClassFixture<Server<Program>>
{
    public OrdersServiceTest(Server<Program> server) : base(server)
    {
        server.InMemoryCollection = (x) =>
        {
            x.Add("Vault:Enabled", "false");
            x.Add("Vault:Address", "http://localhost:8200");
            x.Add("Vault:Token", "root");
            x.Add("Solution", "CodeDesignPlus");
            x.Add("AppName", "my-test");
            x.Add("RabbitMQ:UserName", "guest");
            x.Add("RabbitMQ:Password", "guest");
        };
    }

    [Fact]
    public async Task GetOrder_BidirectionalStreaming_ValidId_ReturnsOrder()
    {
        bool isInvoked = false;
        var orderClient = new Orders.OrdersClient(Channel);

        var orderExpected = OrderAggregate.Create(Guid.NewGuid(), Guid.NewGuid(), "CodeDesignPlus", Guid.NewGuid(), Guid.NewGuid());

        var repository = Services.GetRequiredService<IOrderRepository>();

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
