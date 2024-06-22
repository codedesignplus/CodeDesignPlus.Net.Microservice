namespace CodeDesignPlus.Net.Microservice.Application.Order.Commands.CompleteOrder;

public record CompleteOrderCommand(Guid Id) : IRequest;

public class Validator : AbstractValidator<CompleteOrderCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
    }
}