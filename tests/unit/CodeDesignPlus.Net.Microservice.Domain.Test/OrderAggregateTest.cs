using CodeDesignPlus.Net.Microservice.Domain.ValueObjects;

namespace CodeDesignPlus.Net.Microservice.Domain.Test;

public class OrderAggregateTest
{
    [Fact]
    public void Create_IdIsEmpty_Should_Throw_DomainException()
    {
        // Arrange
        var id = Guid.Empty;
        var idClient = Guid.NewGuid();
        var tenant = Guid.NewGuid();
        var createdBy = Guid.NewGuid();
        var client = ClientValueObject.Create(Guid.NewGuid(), "John Doe", "123456789", "CC");
        var shippingAddress = AddressValueObject.Create("Colombia", "Antioquia", "Medellin", "Cra 80 # 45a 45", 500);

        // Act
        var exception = Assert.Throws<CodeDesignPlusException>(() => OrderAggregate.Create(id, client, shippingAddress, tenant, createdBy));

        // Assert
        Assert.Equal(Errors.IdOrderIsInvalid.GetCode(), exception.Code);
        Assert.Equal(Errors.IdOrderIsInvalid.GetMessage(), exception.Message);
    }

    [Fact]
    public void Create_TenantIsEmpty_Should_Throw_DomainException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var idClient = Guid.NewGuid();
        var tenant = Guid.Empty;
        var createdBy = Guid.NewGuid();
        var client = ClientValueObject.Create(Guid.NewGuid(), "John Doe", "123456789", "CC");
        var shippingAddress = AddressValueObject.Create("Colombia", "Antioquia", "Medellin", "Cra 80 # 45a 45", 500);

        // Act
        var exception = Assert.Throws<CodeDesignPlusException>(() => OrderAggregate.Create(id, client, shippingAddress, tenant, createdBy));

        // Assert
        Assert.Equal(Errors.TenantIsInvalid.GetCode(), exception.Code);
        Assert.Equal(Errors.TenantIsInvalid.GetMessage(), exception.Message);
    }

    [Fact]
    public void Create_ClientIsNull_Should_Throw_DomainException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var idClient = Guid.NewGuid();
        var tenant = Guid.NewGuid();
        var createdBy = Guid.NewGuid();
        var shippingAddress = AddressValueObject.Create("Colombia", "Antioquia", "Medellin", "Cra 80 # 45a 45", 500);

        // Act
        var exception = Assert.Throws<CodeDesignPlusException>(() => OrderAggregate.Create(id, null!, shippingAddress, tenant, createdBy));

        // Assert
        Assert.Equal(Errors.ClientIsNull.GetCode(), exception.Code);
        Assert.Equal(Errors.ClientIsNull.GetMessage(), exception.Message);
    }

    
    [Fact]
    public void Create_AddressIsNull_Should_Throw_DomainException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var idClient = Guid.NewGuid();
        var tenant = Guid.NewGuid();
        var client = ClientValueObject.Create(Guid.NewGuid(), "John Doe", "123456789", "CC");
        var createdBy = Guid.NewGuid();

        // Act
        var exception = Assert.Throws<CodeDesignPlusException>(() => OrderAggregate.Create(id, client, null!, tenant, createdBy));

        // Assert
        Assert.Equal(Errors.AddressIsNull.GetCode(), exception.Code);
        Assert.Equal(Errors.AddressIsNull.GetMessage(), exception.Message);
    }

    [Fact]
    public void Create_Should_Create_OrderAggregate()
    {
        // Arrange
        var id = Guid.NewGuid();
        var tenant = Guid.NewGuid();
        var createdBy = Guid.NewGuid();
        var client = ClientValueObject.Create(Guid.NewGuid(), "John Doe", "123456789", "CC");
        var shippingAddress = AddressValueObject.Create("Colombia", "Antioquia", "Medellin", "Cra 80 # 45a 45", 500);

        // Act
        var orderAggregate = OrderAggregate.Create(id, client, shippingAddress, tenant, createdBy);
        var @event = orderAggregate.GetAndClearEvents()[0] as OrderCreatedDomainEvent;

        // Assert
        Assert.NotNull(orderAggregate);
        Assert.Equal(id, orderAggregate.Id);
        Assert.Equal(client.Id, orderAggregate.Client.Id);
        Assert.Equal(client.Name, orderAggregate.Client.Name);
        Assert.Equal(client.Document, orderAggregate.Client.Document);
        Assert.Equal(client.TypeDocument, orderAggregate.Client.TypeDocument);
        Assert.Equal(shippingAddress.Country, orderAggregate.ShippingAddress.Country);
        Assert.Equal(shippingAddress.State, orderAggregate.ShippingAddress.State);
        Assert.Equal(shippingAddress.City, orderAggregate.ShippingAddress.City);
        Assert.Equal(shippingAddress.Address, orderAggregate.ShippingAddress.Address);
        Assert.Equal(shippingAddress.CodePostal, orderAggregate.ShippingAddress.CodePostal);
        Assert.Equal(tenant, orderAggregate.Tenant);
        Assert.Equal(createdBy, orderAggregate.CreatedBy);
        Assert.Equal(OrderStatus.Created, orderAggregate.Status);

        Assert.NotNull(@event);
        Assert.Equal(id, @event.AggregateId);
        Assert.Equal(client.Id, @event.Client.Id);
        Assert.Equal(client.Name, @event.Client.Name);
        Assert.Equal(client.Document, @event.Client.Document);
        Assert.Equal(client.TypeDocument, @event.Client.TypeDocument);
        Assert.Equal(shippingAddress.Country, @event.ShippingAddress.Country);
        Assert.Equal(shippingAddress.State, @event.ShippingAddress.State);
        Assert.Equal(shippingAddress.City, @event.ShippingAddress.City);
        Assert.Equal(shippingAddress.Address, @event.ShippingAddress.Address);
        Assert.Equal(shippingAddress.CodePostal, @event.ShippingAddress.CodePostal);
        Assert.Equal(tenant, @event.Tenant);
        Assert.Equal(OrderStatus.Created, @event.OrderStatus);
        Assert.NotEqual(0, @event.CreatedAt);
    }

    [Fact]
    public void AddProduct_IdIsEmpty_Should_Throw_DomainException()
    {
        // Arrange
        var createdBy = Guid.NewGuid();
        var updatedBy = Guid.NewGuid();
        var client = ClientValueObject.Create(Guid.NewGuid(), "John Doe", "123456789", "CC");
        var shippingAddress = AddressValueObject.Create("Colombia", "Antioquia", "Medellin", "Cra 80 # 45a 45", 500);
        var orderAggregate = OrderAggregate.Create(Guid.NewGuid(),client, shippingAddress, Guid.NewGuid(), createdBy);
        var id = Guid.Empty;
        var name = "Product 1";
        var description = "Product 1 description";
        var price = 10L;
        var quantity = 2;

        // Act
        var exception = Assert.Throws<CodeDesignPlusException>(() => orderAggregate.AddProduct(id, name, description, price, quantity, updatedBy));

        // Assert
        Assert.Equal(Errors.IdProductIsInvalid.GetCode(), exception.Code);
        Assert.Equal(Errors.IdProductIsInvalid.GetMessage(), exception.Message);
    }

    [Fact]
    public void AddProduct_NameIsEmpty_Should_Throw_DomainException()
    {
        // Arrange
        var createdBy = Guid.NewGuid();
        var updatedBy = Guid.NewGuid();
        var client = ClientValueObject.Create(Guid.NewGuid(), "John Doe", "123456789", "CC");
        var shippingAddress = AddressValueObject.Create("Colombia", "Antioquia", "Medellin", "Cra 80 # 45a 45", 500);
        var orderAggregate = OrderAggregate.Create(Guid.NewGuid(), client, shippingAddress, Guid.NewGuid(), createdBy);
        var id = Guid.NewGuid();
        var name = string.Empty;
        var description = "Product 1 description";
        var price = 10L;
        var quantity = 2;

        // Act
        var exception = Assert.Throws<CodeDesignPlusException>(() => orderAggregate.AddProduct(id, name, description, price, quantity, updatedBy));

        // Assert
        Assert.Equal(Errors.NameProductIsInvalid.GetCode(), exception.Code);
        Assert.Equal(Errors.NameProductIsInvalid.GetMessage(), exception.Message);
    }

    [Fact]
    public void AddProduct_PriceIsLessThanZero_Should_Throw_DomainException()
    {
        // Arrange
        var createdBy = Guid.NewGuid();
        var updatedBy = Guid.NewGuid();
        var client = ClientValueObject.Create(Guid.NewGuid(), "John Doe", "123456789", "CC");
        var shippingAddress = AddressValueObject.Create("Colombia", "Antioquia", "Medellin", "Cra 80 # 45a 45", 500);
        var orderAggregate = OrderAggregate.Create(Guid.NewGuid(), client, shippingAddress, Guid.NewGuid(), createdBy);
        var id = Guid.NewGuid();
        var name = "Product 1";
        var description = "Product 1 description";
        var price = -1L;
        var quantity = 2;

        // Act
        var exception = Assert.Throws<CodeDesignPlusException>(() => orderAggregate.AddProduct(id, name, description, price, quantity, updatedBy));

        // Assert
        Assert.Equal(Errors.PriceProductIsInvalid.GetCode(), exception.Code);
        Assert.Equal(Errors.PriceProductIsInvalid.GetMessage(), exception.Message);
    }

    [Fact]
    public void AddProduct_QuantityIsLessThanZero_Should_Throw_DomainException()
    {
        // Arrange
        var createdBy = Guid.NewGuid();
        var updatedBy = Guid.NewGuid();
        var client = ClientValueObject.Create(Guid.NewGuid(), "John Doe", "123456789", "CC");
        var shippingAddress = AddressValueObject.Create("Colombia", "Antioquia", "Medellin", "Cra 80 # 45a 45", 500);
        var orderAggregate = OrderAggregate.Create(Guid.NewGuid(), client, shippingAddress, Guid.NewGuid(), createdBy);
        var id = Guid.NewGuid();
        var name = "Product 1";
        var description = "Product 1 description";
        var price = 10L;
        var quantity = -1;

        // Act
        var exception = Assert.Throws<CodeDesignPlusException>(() => orderAggregate.AddProduct(id, name, description, price, quantity, updatedBy));

        // Assert
        Assert.Equal(Errors.QuantityProductIsInvalid.GetCode(), exception.Code);
        Assert.Equal(Errors.QuantityProductIsInvalid.GetMessage(), exception.Message);
    }

    [Fact]
    public void AddProduct_Should_Add_Product_To_OrderAggregate()
    {
        // Arrange
        var createdBy = Guid.NewGuid();
        var updatedBy = Guid.NewGuid();
        var client = ClientValueObject.Create(Guid.NewGuid(), "John Doe", "123456789", "CC");
        var shippingAddress = AddressValueObject.Create("Colombia", "Antioquia", "Medellin", "Cra 80 # 45a 45", 500);
        var orderAggregate = OrderAggregate.Create(Guid.NewGuid(), client, shippingAddress, Guid.NewGuid(), createdBy);
        var productId = Guid.NewGuid();
        var name = "Product 1";
        var description = "Product 1 description";
        var price = 10L;
        var quantity = 2;

        // Act
        orderAggregate.AddProduct(productId, name, description, price, quantity, updatedBy);
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

        Assert.NotNull(orderAggregate.UpdatedAt);
        Assert.Equal(updatedBy, orderAggregate.UpdatedBy);
        Assert.NotEqual(orderAggregate.CreatedBy, orderAggregate.UpdatedBy);
    }

    [Fact]
    public void RemoveProduct_IdIsEmpty_Should_Throw_DomainException()
    {
        // Arrange
        var createdBy = Guid.NewGuid();
        var updatedBy = Guid.NewGuid();
        var client = ClientValueObject.Create(Guid.NewGuid(), "John Doe", "123456789", "CC");
        var shippingAddress = AddressValueObject.Create("Colombia", "Antioquia", "Medellin", "Cra 80 # 45a 45", 500);
        var orderAggregate = OrderAggregate.Create(Guid.NewGuid(), client, shippingAddress, Guid.NewGuid(), createdBy);
        var id = Guid.Empty;

        // Act
        var exception = Assert.Throws<CodeDesignPlusException>(() => orderAggregate.RemoveProduct(id, updatedBy));

        // Assert
        Assert.Equal(Errors.IdProductIsInvalid.GetCode(), exception.Code);
        Assert.Equal(Errors.IdProductIsInvalid.GetMessage(), exception.Message);
    }

    [Fact]
    public void RemoveProduct_ProductNotFound_Should_Throw_DomainException()
    {
        // Arrange
        var createdBy = Guid.NewGuid();
        var updatedBy = Guid.NewGuid();
        var client = ClientValueObject.Create(Guid.NewGuid(), "John Doe", "123456789", "CC");
        var shippingAddress = AddressValueObject.Create("Colombia", "Antioquia", "Medellin", "Cra 80 # 45a 45", 500);
        var orderAggregate = OrderAggregate.Create(Guid.NewGuid(), client, shippingAddress, Guid.NewGuid(), createdBy);
        var productId = Guid.NewGuid();

        // Act
        var exception = Assert.Throws<CodeDesignPlusException>(() => orderAggregate.RemoveProduct(productId, updatedBy));

        // Assert
        Assert.Equal(Errors.ProductNotFound.GetCode(), exception.Code);
        Assert.Equal(Errors.ProductNotFound.GetMessage(), exception.Message);
    }

    [Fact]
    public void RemoveProduct_Should_Remove_Product_From_OrderAggregate()
    {
        // Arrange
        var createdBy = Guid.NewGuid();
        var updatedBy = Guid.NewGuid();
        var client = ClientValueObject.Create(Guid.NewGuid(), "John Doe", "123456789", "CC");
        var shippingAddress = AddressValueObject.Create("Colombia", "Antioquia", "Medellin", "Cra 80 # 45a 45", 500);
        var orderAggregate = OrderAggregate.Create(Guid.NewGuid(), client, shippingAddress, Guid.NewGuid(), createdBy);
        var productId = Guid.NewGuid();
        var name = "Product 1";
        var description = "Product 1 description";
        var price = 10L;
        var quantity = 2;
        orderAggregate.AddProduct(productId, name, description, price, quantity, Guid.NewGuid());

        // Act
        orderAggregate.RemoveProduct(productId, updatedBy);
        var @event = orderAggregate.GetAndClearEvents().FirstOrDefault(x => x is ProductRemovedFromOrderDomainEvent) as ProductRemovedFromOrderDomainEvent;

        // Assert
        Assert.DoesNotContain(orderAggregate.Products, p => p.Id == productId);

        Assert.NotNull(@event);
        Assert.Equal(orderAggregate.Id, @event.AggregateId);
        Assert.Equal(productId, @event.ProductId);

        Assert.NotNull(orderAggregate.UpdatedAt);
        Assert.Equal(updatedBy, orderAggregate.UpdatedBy);
        Assert.NotEqual(orderAggregate.CreatedBy, orderAggregate.UpdatedBy);
    }

    [Fact]
    public void UpdateProductQuantity_IdIsEmpty_Should_Throw_DomainException()
    {
        // Arrange
        var createdBy = Guid.NewGuid();
        var updatedBy = Guid.NewGuid();
        var client = ClientValueObject.Create(Guid.NewGuid(), "John Doe", "123456789", "CC");
        var shippingAddress = AddressValueObject.Create("Colombia", "Antioquia", "Medellin", "Cra 80 # 45a 45", 500);
        var orderAggregate = OrderAggregate.Create(Guid.NewGuid(), client, shippingAddress, Guid.NewGuid(), createdBy);
        var id = Guid.Empty;
        var newQuantity = 5;

        // Act
        var exception = Assert.Throws<CodeDesignPlusException>(() => orderAggregate.UpdateProductQuantity(id, newQuantity, updatedBy));

        // Assert
        Assert.Equal(Errors.IdProductIsInvalid.GetCode(), exception.Code);
        Assert.Equal(Errors.IdProductIsInvalid.GetMessage(), exception.Message);
    }

    [Fact]
    public void UpdateProductQuantity_QuantityIsLessThanZero_Should_Throw_DomainException()
    {
        // Arrange
        var createdBy = Guid.NewGuid();
        var updatedBy = Guid.NewGuid();
        var client = ClientValueObject.Create(Guid.NewGuid(), "John Doe", "123456789", "CC");
        var shippingAddress = AddressValueObject.Create("Colombia", "Antioquia", "Medellin", "Cra 80 # 45a 45", 500);
        var orderAggregate = OrderAggregate.Create(Guid.NewGuid(), client, shippingAddress, Guid.NewGuid(), createdBy);
        var productId = Guid.NewGuid();
        var newQuantity = -1;

        // Act
        var exception = Assert.Throws<CodeDesignPlusException>(() => orderAggregate.UpdateProductQuantity(productId, newQuantity, updatedBy));

        // Assert
        Assert.Equal(Errors.QuantityProductIsInvalid.GetCode(), exception.Code);
        Assert.Equal(Errors.QuantityProductIsInvalid.GetMessage(), exception.Message);
    }

    [Fact]
    public void UpdateProductQuantity_Should_Update_Product_Quantity_In_OrderAggregate()
    {
        // Arrange
        var createdBy = Guid.NewGuid();
        var updatedBy = Guid.NewGuid();
        var client = ClientValueObject.Create(Guid.NewGuid(), "John Doe", "123456789", "CC");
        var shippingAddress = AddressValueObject.Create("Colombia", "Antioquia", "Medellin", "Cra 80 # 45a 45", 500);
        var orderAggregate = OrderAggregate.Create(Guid.NewGuid(), client, shippingAddress, Guid.NewGuid(), createdBy);
        var productId = Guid.NewGuid();
        var name = "Product 1";
        var description = "Product 1 description";
        var price = 10L;
        var quantity = 2;
        orderAggregate.AddProduct(productId, name, description, price, quantity, Guid.NewGuid());
        var newQuantity = 5;

        // Act
        orderAggregate.UpdateProductQuantity(productId, newQuantity, updatedBy);
        var @event = orderAggregate.GetAndClearEvents().FirstOrDefault(x => x is ProductQuantityUpdatedDomainEvent) as ProductQuantityUpdatedDomainEvent;

        // Assert
        var product = orderAggregate.Products.First(p => p.Id == productId);
        Assert.NotNull(product);
        Assert.Equal(newQuantity, product.Quantity);

        Assert.NotNull(@event);
        Assert.Equal(orderAggregate.Id, @event.AggregateId);
        Assert.Equal(productId, @event.ProductId);
        Assert.Equal(newQuantity, @event.NewQuantity);

        Assert.NotNull(orderAggregate.UpdatedAt);
        Assert.Equal(updatedBy, orderAggregate.UpdatedBy);
        Assert.NotEqual(orderAggregate.CreatedBy, orderAggregate.UpdatedBy);
    }

    [Fact]
    public void CompleteOrder_StatusIsCancelled_ShouldThrow_DomainException()
    {
        // Arrange
        var createdBy = Guid.NewGuid();
        var updatedBy = Guid.NewGuid();
        var client = ClientValueObject.Create(Guid.NewGuid(), "John Doe", "123456789", "CC");
        var shippingAddress = AddressValueObject.Create("Colombia", "Antioquia", "Medellin", "Cra 80 # 45a 45", 500);
        var orderAggregate = OrderAggregate.Create(Guid.NewGuid(), client, shippingAddress, Guid.NewGuid(), createdBy);
        orderAggregate.CancelOrder("Out of stock", Guid.NewGuid());

        // Act
        var exception = Assert.Throws<CodeDesignPlusException>(() => orderAggregate.CompleteOrder(updatedBy));

        // Assert
        Assert.Equal(Errors.OrderAlreadyCancelled.GetCode(), exception.Code);
        Assert.Equal(Errors.OrderAlreadyCancelled.GetMessage(), exception.Message);
    }

    [Fact]
    public void CompleteOrder_StatusIsCompleted_ShouldThrow_DomainException()
    {
        // Arrange
        var createdBy = Guid.NewGuid();
        var updatedBy = Guid.NewGuid();
        var client = ClientValueObject.Create(Guid.NewGuid(), "John Doe", "123456789", "CC");
        var shippingAddress = AddressValueObject.Create("Colombia", "Antioquia", "Medellin", "Cra 80 # 45a 45", 500);
        var orderAggregate = OrderAggregate.Create(Guid.NewGuid(), client, shippingAddress, Guid.NewGuid(), createdBy);
        orderAggregate.CompleteOrder(Guid.NewGuid());

        // Act
        var exception = Assert.Throws<CodeDesignPlusException>(() => orderAggregate.CompleteOrder(updatedBy));

        // Assert
        Assert.Equal(Errors.OrderAlreadyCompleted.GetCode(), exception.Code);
        Assert.Equal(Errors.OrderAlreadyCompleted.GetMessage(), exception.Message);
    }

    [Fact]
    public void CompleteOrder_Should_Complete_OrderAggregate()
    {
        // Arrange
        var createdBy = Guid.NewGuid();
        var updatedBy = Guid.NewGuid();
        var client = ClientValueObject.Create(Guid.NewGuid(), "John Doe", "123456789", "CC");
        var shippingAddress = AddressValueObject.Create("Colombia", "Antioquia", "Medellin", "Cra 80 # 45a 45", 500);
        var orderAggregate = OrderAggregate.Create(Guid.NewGuid(), client, shippingAddress, Guid.NewGuid(), createdBy);

        // Act
        orderAggregate.CompleteOrder(updatedBy);
        var @event = orderAggregate.GetAndClearEvents().FirstOrDefault(x => x is OrderCompletedDomainEvent) as OrderCompletedDomainEvent;

        // Assert
        Assert.Equal(OrderStatus.Completed, orderAggregate.Status);
        Assert.NotNull(orderAggregate.CompletedAt);

        Assert.NotNull(@event);
        Assert.Equal(orderAggregate.Id, @event.AggregateId);
        Assert.NotEqual(0, @event.CompletedAt);

        Assert.NotNull(orderAggregate.UpdatedAt);
        Assert.Equal(updatedBy, orderAggregate.UpdatedBy);
        Assert.NotEqual(orderAggregate.CreatedBy, orderAggregate.UpdatedBy);
    }

    [Fact]
    public void CancelOrder_StatusIsCancelled_ShouldThrow_DomainException()
    {
        // Arrange
        var createdBy = Guid.NewGuid();
        var updatedBy = Guid.NewGuid();
        var client = ClientValueObject.Create(Guid.NewGuid(), "John Doe", "123456789", "CC");
        var shippingAddress = AddressValueObject.Create("Colombia", "Antioquia", "Medellin", "Cra 80 # 45a 45", 500);
        var orderAggregate = OrderAggregate.Create(Guid.NewGuid(), client, shippingAddress, Guid.NewGuid(), createdBy);
        orderAggregate.CancelOrder("Out of stock", Guid.NewGuid());

        // Act
        var exception = Assert.Throws<CodeDesignPlusException>(() => orderAggregate.CancelOrder("Out of stock", updatedBy));

        // Assert
        Assert.Equal(Errors.OrderAlreadyCancelled.GetCode(), exception.Code);
        Assert.Equal(Errors.OrderAlreadyCancelled.GetMessage(), exception.Message);
    }

    [Fact]
    public void CancelOrder_Should_Cancel_OrderAggregate()
    {
        // Arrange
        var createdBy = Guid.NewGuid();
        var updatedBy = Guid.NewGuid();
        var client = ClientValueObject.Create(Guid.NewGuid(), "John Doe", "123456789", "CC");
        var shippingAddress = AddressValueObject.Create("Colombia", "Antioquia", "Medellin", "Cra 80 # 45a 45", 500);
        var orderAggregate = OrderAggregate.Create(Guid.NewGuid(), client, shippingAddress, Guid.NewGuid(), createdBy);
        var reason = "Out of stock";

        // Act
        orderAggregate.CancelOrder(reason, updatedBy);
        var @event = orderAggregate.GetAndClearEvents().FirstOrDefault(x => x is OrderCancelledDomainEvent) as OrderCancelledDomainEvent;

        // Assert
        Assert.Equal(OrderStatus.Cancelled, orderAggregate.Status);
        Assert.NotNull(orderAggregate.CancelledAt);
        Assert.Equal(reason, orderAggregate.ReasonForCancellation);

        Assert.NotNull(@event);
        Assert.Equal(orderAggregate.Id, @event.AggregateId);
        Assert.Equal(reason, @event.Reason);

        Assert.NotNull(orderAggregate.UpdatedAt);
        Assert.Equal(updatedBy, orderAggregate.UpdatedBy);
        Assert.NotEqual(orderAggregate.CreatedBy, orderAggregate.UpdatedBy);
    }
}
