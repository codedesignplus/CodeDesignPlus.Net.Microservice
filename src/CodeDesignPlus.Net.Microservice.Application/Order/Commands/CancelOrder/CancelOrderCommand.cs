namespace CodeDesignPlus.Net.Microservice.Application.Order.Commands.CancelOrder;

[DtoGenerator]
public record CancelOrderCommand(Guid Id, string Reason) : IRequest;

public class Validator : AbstractValidator<CancelOrderCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.Reason).NotEmpty().NotNull();
    }
}