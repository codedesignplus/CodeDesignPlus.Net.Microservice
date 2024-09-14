using CodeDesignPlus.Net.Exceptions;
using CodeDesignPlus.Net.Exceptions.Extensions;

namespace CodeDesignPlus.Net.Microservice.Domain.Test;

public class OrderAggregateTest
{
    [Fact]
    public void Create_IdIsEmpty_Should_Throw_DomainException()
    {
        // Arrange
        var id = Guid.Empty;
        var idClient = Guid.NewGuid();
        var nameClient = "John Doe";
        var tenant = Guid.NewGuid();

        // Act
        var exception = Assert.Throws<CodeDesignPlusException>(() => OrderAggregate.Create(id, idClient, nameClient, tenant));

        // Assert
        Assert.Equal(Errors.IdOrderIsInvalid.GetCode(), exception.Code);
        Assert.Equal(Errors.IdOrderIsInvalid.GetMessage(), exception.Message);
    }

    [Fact]
    public void Create_IdClientIsEmpty_Should_Throw_DomainException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var idClient = Guid.Empty;
        var nameClient = "John Doe";
        var tenant = Guid.NewGuid();

        // Act
        var exception = Assert.Throws<CodeDesignPlusException>(() => OrderAggregate.Create(id, idClient, nameClient, tenant));

        // Assert
        Assert.Equal(Errors.IdClientIsInvalid.GetCode(), exception.Code);
        Assert.Equal(Errors.IdClientIsInvalid.GetMessage(), exception.Message);
    }

    [Fact]
    public void Create_TenantIsEmpty_Should_Throw_DomainException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var idClient = Guid.NewGuid();
        var nameClient = "John Doe";
        var tenant = Guid.Empty;

        // Act
        var exception = Assert.Throws<CodeDesignPlusException>(() => OrderAggregate.Create(id, idClient, nameClient, tenant));

        // Assert
        Assert.Equal(Errors.TenantIsInvalid.GetCode(), exception.Code);
        Assert.Equal(Errors.TenantIsInvalid.GetMessage(), exception.Message);
    }

    [Fact]
    public void Create_NameClientIsEmpty_Should_Throw_DomainException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var idClient = Guid.NewGuid();
        var nameClient = string.Empty;
        var tenant = Guid.NewGuid();

        // Act
        var exception = Assert.Throws<CodeDesignPlusException>(() => OrderAggregate.Create(id, idClient, nameClient, tenant));

        // Assert
        Assert.Equal(Errors.NameClientIsInvalid.GetCode(), exception.Code);
        Assert.Equal(Errors.NameClientIsInvalid.GetMessage(), exception.Message);
    }

    [Fact]
    public void Create_Should_Create_OrderAggregate()
    {
        // Arrange
        var id = Guid.NewGuid();
        var idClient = Guid.NewGuid();
        var nameClient = "John Doe";
        var tenant = Guid.NewGuid();

        // Act
        var orderAggregate = OrderAggregate.Create(id, idClient, nameClient, tenant);
        var @event = orderAggregate.GetAndClearEvents()[0] as OrderCreatedDomainEvent;

        // Assert
        Assert.NotNull(orderAggregate);
        Assert.Equal(id, orderAggregate.Id);
        Assert.Equal(idClient, orderAggregate.Client.Id);
        Assert.Equal(nameClient, orderAggregate.Client.Name);
        Assert.Equal(tenant, orderAggregate.Tenant);
        Assert.Equal(OrderStatus.Created, orderAggregate.Status);

        Assert.NotNull(@event);
        Assert.Equal(id, @event.AggregateId);
        Assert.Equal(idClient, @event.Client.Id);
        Assert.Equal(nameClient, @event.Client.Name);
        Assert.Equal(tenant, @event.Tenant);
        Assert.Equal(OrderStatus.Created, @event.OrderStatus);
        Assert.NotEqual(0, @event.CreatedAt);
    }

    [Fact]
    public void AddProduct_IdIsEmpty_Should_Throw_DomainException()
    {
        // Arrange
        var orderAggregate = OrderAggregate.Create(Guid.NewGuid(), Guid.NewGuid(), "John Doe", Guid.NewGuid());
        var id = Guid.Empty;
        var name = "Product 1";
        var description = "Product 1 description";
        var price = 10L;
        var quantity = 2;

        // Act
        var exception = Assert.Throws<CodeDesignPlusException>(() => orderAggregate.AddProduct(id, name, description, price, quantity));

        // Assert
        Assert.Equal(Errors.IdProductIsInvalid.GetCode(), exception.Code);
        Assert.Equal(Errors.IdProductIsInvalid.GetMessage(), exception.Message);
    }

    [Fact]
    public void AddProduct_NameIsEmpty_Should_Throw_DomainException()
    {
        // Arrange
        var orderAggregate = OrderAggregate.Create(Guid.NewGuid(), Guid.NewGuid(), "John Doe", Guid.NewGuid());
        var id = Guid.NewGuid();
        var name = string.Empty;
        var description = "Product 1 description";
        var price = 10L;
        var quantity = 2;

        // Act
        var exception = Assert.Throws<CodeDesignPlusException>(() => orderAggregate.AddProduct(id, name, description, price, quantity));

        // Assert
        Assert.Equal(Errors.NameProductIsInvalid.GetCode(), exception.Code);
        Assert.Equal(Errors.NameProductIsInvalid.GetMessage(), exception.Message);
    }

    [Fact]
    public void AddProduct_PriceIsLessThanZero_Should_Throw_DomainException()
    {
        // Arrange
        var orderAggregate = OrderAggregate.Create(Guid.NewGuid(), Guid.NewGuid(), "John Doe", Guid.NewGuid());
        var id = Guid.NewGuid();
        var name = "Product 1";
        var description = "Product 1 description";
        var price = -1L;
        var quantity = 2;

        // Act
        var exception = Assert.Throws<CodeDesignPlusException>(() => orderAggregate.AddProduct(id, name, description, price, quantity));

        // Assert
        Assert.Equal(Errors.PriceProductIsInvalid.GetCode(), exception.Code);
        Assert.Equal(Errors.PriceProductIsInvalid.GetMessage(), exception.Message);
    }

    [Fact]
    public void AddProduct_QuantityIsLessThanZero_Should_Throw_DomainException()
    {
        // Arrange
        var orderAggregate = OrderAggregate.Create(Guid.NewGuid(), Guid.NewGuid(), "John Doe", Guid.NewGuid());
        var id = Guid.NewGuid();
        var name = "Product 1";
        var description = "Product 1 description";
        var price = 10L;
        var quantity = -1;

        // Act
        var exception = Assert.Throws<CodeDesignPlusException>(() => orderAggregate.AddProduct(id, name, description, price, quantity));

        // Assert
        Assert.Equal(Errors.QuantityProductIsInvalid.GetCode(), exception.Code);
        Assert.Equal(Errors.QuantityProductIsInvalid.GetMessage(), exception.Message);
    }

    [Fact]
    public void AddProduct_Should_Add_Product_To_OrderAggregate()
    {
        // Arrange
        var orderAggregate = OrderAggregate.Create(Guid.NewGuid(), Guid.NewGuid(), "John Doe", Guid.NewGuid());
        var productId = Guid.NewGuid();
        var name = "Product 1";
        var description = "Product 1 description";
        var price = 10L;
        var quantity = 2;

        // Act
        orderAggregate.AddProduct(productId, name, description, price, quantity);
        var @event = orderAggregate.GetAndClearEvents().FirstOrDefault(x => x is ProductAddedToOrderDomainEvent) as ProductAddedToOrderDomainEvent;

        // Assert
        var product = orderAggregate.Products.First(p => p.Id == productId);
        Assert.NotNull(product);
        Assert.Equal(name, product.Name);
        Assert.Equal(description, product.Description);
        Assert.Equal(price, product.Price);
        Assert.Equal(quantity, product.Quantity);

        Assert.NotNull(@event);
        Assert.Equal(orderAggregate.Id, @event.AggregateId);
        Assert.Equal(quantity, @event.Quantity);
        Assert.Equal(productId, @event.Product.Id);
        Assert.Equal(name, @event.Product.Name);
        Assert.Equal(description, @event.Product.Description);
        Assert.Equal(price, @event.Product.Price);
    }

    [Fact]
    public void RemoveProduct_IdIsEmpty_Should_Throw_DomainException()
    {
        // Arrange
        var orderAggregate = OrderAggregate.Create(Guid.NewGuid(), Guid.NewGuid(), "John Doe", Guid.NewGuid());
        var id = Guid.Empty;

        // Act
        var exception = Assert.Throws<CodeDesignPlusException>(() => orderAggregate.RemoveProduct(id));

        // Assert
        Assert.Equal(Errors.IdProductIsInvalid.GetCode(), exception.Code);
        Assert.Equal(Errors.IdProductIsInvalid.GetMessage(), exception.Message);
    }

    [Fact]
    public void RemoveProduct_ProductNotFound_Should_Throw_DomainException()
    {
        // Arrange
        var orderAggregate = OrderAggregate.Create(Guid.NewGuid(), Guid.NewGuid(), "John Doe", Guid.NewGuid());
        var productId = Guid.NewGuid();

        // Act
        var exception = Assert.Throws<CodeDesignPlusException>(() => orderAggregate.RemoveProduct(productId));

        // Assert
        Assert.Equal(Errors.ProductNotFound.GetCode(), exception.Code);
        Assert.Equal(Errors.ProductNotFound.GetMessage(), exception.Message);
    }

    [Fact]
    public void RemoveProduct_Should_Remove_Product_From_OrderAggregate()
    {
        // Arrange
        var orderAggregate = OrderAggregate.Create(Guid.NewGuid(), Guid.NewGuid(), "John Doe", Guid.NewGuid());
        var productId = Guid.NewGuid();
        var name = "Product 1";
        var description = "Product 1 description";
        var price = 10L;
        var quantity = 2;
        orderAggregate.AddProduct(productId, name, description, price, quantity);

        // Act
        orderAggregate.RemoveProduct(productId);
        var @event = orderAggregate.GetAndClearEvents().FirstOrDefault(x => x is ProductRemovedFromOrderDomainEvent) as ProductRemovedFromOrderDomainEvent;

        // Assert
        Assert.DoesNotContain(orderAggregate.Products, p => p.Id == productId);

        Assert.NotNull(@event);
        Assert.Equal(orderAggregate.Id, @event.AggregateId);
        Assert.Equal(productId, @event.ProductId);
    }

    [Fact]
    public void UpdateProductQuantity_IdIsEmpty_Should_Throw_DomainException()
    {
        // Arrange
        var orderAggregate = OrderAggregate.Create(Guid.NewGuid(), Guid.NewGuid(), "John Doe", Guid.NewGuid());
        var id = Guid.Empty;
        var newQuantity = 5;

        // Act
        var exception = Assert.Throws<CodeDesignPlusException>(() => orderAggregate.UpdateProductQuantity(id, newQuantity));

        // Assert
        Assert.Equal(Errors.IdProductIsInvalid.GetCode(), exception.Code);
        Assert.Equal(Errors.IdProductIsInvalid.GetMessage(), exception.Message);
    }

    [Fact]
    public void UpdateProductQuantity_QuantityIsLessThanZero_Should_Throw_DomainException()
    {
        // Arrange
        var orderAggregate = OrderAggregate.Create(Guid.NewGuid(), Guid.NewGuid(), "John Doe", Guid.NewGuid());
        var productId = Guid.NewGuid();
        var newQuantity = -1;

        // Act
        var exception = Assert.Throws<CodeDesignPlusException>(() => orderAggregate.UpdateProductQuantity(productId, newQuantity));

        // Assert
        Assert.Equal(Errors.QuantityProductIsInvalid.GetCode(), exception.Code);
        Assert.Equal(Errors.QuantityProductIsInvalid.GetMessage(), exception.Message);
    }

    [Fact]
    public void UpdateProductQuantity_Should_Update_Product_Quantity_In_OrderAggregate()
    {
        // Arrange
        var orderAggregate = OrderAggregate.Create(Guid.NewGuid(), Guid.NewGuid(), "John Doe", Guid.NewGuid());
        var productId = Guid.NewGuid();
        var name = "Product 1";
        var description = "Product 1 description";
        var price = 10L;
        var quantity = 2;
        orderAggregate.AddProduct(productId, name, description, price, quantity);
        var newQuantity = 5;

        // Act
        orderAggregate.UpdateProductQuantity(productId, newQuantity);
        var @event = orderAggregate.GetAndClearEvents().FirstOrDefault(x => x is ProductQuantityUpdatedDomainEvent) as ProductQuantityUpdatedDomainEvent;

        // Assert
        var product = orderAggregate.Products.First(p => p.Id == productId);
        Assert.NotNull(product);
        Assert.Equal(newQuantity, product.Quantity);

        Assert.NotNull(@event);
        Assert.Equal(orderAggregate.Id, @event.AggregateId);
        Assert.Equal(productId, @event.ProductId);
        Assert.Equal(newQuantity, @event.NewQuantity);
    }

    [Fact]
    public void CompleteOrder_StatusIsCancelled_ShouldThrow_DomainException()
    {
        // Arrange
        var orderAggregate = OrderAggregate.Create(Guid.NewGuid(), Guid.NewGuid(), "John Doe", Guid.NewGuid());
        orderAggregate.CancelOrder("Out of stock");

        // Act
        var exception = Assert.Throws<CodeDesignPlusException>(() => orderAggregate.CompleteOrder());

        // Assert
        Assert.Equal(Errors.OrderAlreadyCancelled.GetCode(), exception.Code);
        Assert.Equal(Errors.OrderAlreadyCancelled.GetMessage(), exception.Message);
    }

    [Fact]
    public void CompleteOrder_StatusIsCompleted_ShouldThrow_DomainException()
    {
        // Arrange
        var orderAggregate = OrderAggregate.Create(Guid.NewGuid(), Guid.NewGuid(), "John Doe", Guid.NewGuid());
        orderAggregate.CompleteOrder();

        // Act
        var exception = Assert.Throws<CodeDesignPlusException>(() => orderAggregate.CompleteOrder());

        // Assert
        Assert.Equal(Errors.OrderAlreadyCompleted.GetCode(), exception.Code);
        Assert.Equal(Errors.OrderAlreadyCompleted.GetMessage(), exception.Message);
    }

    [Fact]
    public void CompleteOrder_Should_Complete_OrderAggregate()
    {
        // Arrange
        var orderAggregate = OrderAggregate.Create(Guid.NewGuid(), Guid.NewGuid(), "John Doe", Guid.NewGuid());

        // Act
        orderAggregate.CompleteOrder();
        var @event = orderAggregate.GetAndClearEvents().FirstOrDefault(x => x is OrderCompletedDomainEvent) as OrderCompletedDomainEvent;

        // Assert
        Assert.Equal(OrderStatus.Completed, orderAggregate.Status);
        Assert.NotNull(orderAggregate.CompletedAt);

        Assert.NotNull(@event);
        Assert.Equal(orderAggregate.Id, @event.AggregateId);
        Assert.NotEqual(0, @event.CompletedAt);
    }

    [Fact]
    public void CancelOrder_StatusIsCancelled_ShouldThrow_DomainException()
    {
        // Arrange
        var orderAggregate = OrderAggregate.Create(Guid.NewGuid(), Guid.NewGuid(), "John Doe", Guid.NewGuid());
        orderAggregate.CancelOrder("Out of stock");

        // Act
        var exception = Assert.Throws<CodeDesignPlusException>(() => orderAggregate.CancelOrder("Out of stock"));

        // Assert
        Assert.Equal(Errors.OrderAlreadyCancelled.GetCode(), exception.Code);
        Assert.Equal(Errors.OrderAlreadyCancelled.GetMessage(), exception.Message);
    }

    [Fact]
    public void CancelOrder_Should_Cancel_OrderAggregate()
    {
        // Arrange
        var orderAggregate = OrderAggregate.Create(Guid.NewGuid(), Guid.NewGuid(), "John Doe", Guid.NewGuid());
        var reason = "Out of stock";

        // Act
        orderAggregate.CancelOrder(reason);
        var @event = orderAggregate.GetAndClearEvents().FirstOrDefault(x => x is OrderCancelledDomainEvent) as OrderCancelledDomainEvent;

        // Assert
        Assert.Equal(OrderStatus.Cancelled, orderAggregate.Status);
        Assert.NotNull(orderAggregate.CancelledAt);
        Assert.Equal(reason, orderAggregate.ReasonForCancellation);

        Assert.NotNull(@event);
        Assert.Equal(orderAggregate.Id, @event.AggregateId);
        Assert.Equal(reason, @event.Reason);
    }
}
