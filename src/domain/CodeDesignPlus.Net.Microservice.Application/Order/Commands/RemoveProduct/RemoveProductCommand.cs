namespace CodeDesignPlus.Net.Microservice.Application.Order.Commands.RemoveProduct;

public record RemoveProductCommand(Guid Id, Guid ProductId) : IRequest;

public class Validator : AbstractValidator<RemoveProductCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.ProductId).NotEmpty().NotNull();
    }
}