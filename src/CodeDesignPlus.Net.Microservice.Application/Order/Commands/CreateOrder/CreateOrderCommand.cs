using CodeDesignPlus.Net.Generator;
using CodeDesignPlus.Net.Microservice.Application.Order.DataTransferObjects;
using FluentValidation;
using MediatR;

namespace CodeDesignPlus.Net.Microservice.Application.Order.Commands.CreateOrder;

[DtoGenerator]
public record CreateOrderCommand(Guid Id, ClientDto Client, Guid Tenant) : IRequest;

public class Validator : AbstractValidator<CreateOrderCommand>
{
    public Validator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Client)
            .NotNull();

        RuleFor(x => x.Tenant)
            .NotEmpty()
            .NotNull();
    }
}
