namespace CodeDesignPlus.Net.Microservice.Application.Order.Commands.UpdateQuantityProduct;

[DtoGenerator]
public record UpdateQuantityProductCommand(Guid Id, Guid ProductId, int Quantity) : IRequest;

public class Validator : AbstractValidator<UpdateQuantityProductCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.ProductId).NotEmpty().NotNull();
        RuleFor(x => x.Quantity).GreaterThan(0);
    }
}