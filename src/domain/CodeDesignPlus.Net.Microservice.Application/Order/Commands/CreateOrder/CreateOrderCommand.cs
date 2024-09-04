
namespace CodeDesignPlus.Net.Microservice.Application.Order.Commands.CreateOrder;

[DtoGenerator]
public record CreateOrderCommand(Guid Id, ClientDto Client) : IRequest;

public class Validator : AbstractValidator<CreateOrderCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
        RuleFor(x => x.Client)
            .NotNull()
            .DependentRules(() =>
            {
                RuleFor(x => x.Client.Id).NotEmpty().NotNull();
                RuleFor(x => x.Client.Name).NotEmpty().NotNull();
            });
    }
}
