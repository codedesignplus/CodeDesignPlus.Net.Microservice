namespace CodeDesignPlus.Net.Microservice.Application.Order.Commands.AddProductToOrder;

[DtoGenerator]
public record AddProductToOrderCommand(Guid Id, Guid IdProduct, string Name, string Description, long Price, int Quantity) : IRequest;

public class Validator : AbstractValidator<AddProductToOrderCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.IdProduct).NotEmpty().NotNull();
        RuleFor(x => x.Name).NotEmpty().NotNull();
        RuleFor(x => x.Description).NotEmpty().NotNull();
        RuleFor(x => x.Price).NotEmpty().NotNull().GreaterThan(0);
        RuleFor(x => x.Quantity).NotEmpty().NotNull().GreaterThan(0);
    }
}
