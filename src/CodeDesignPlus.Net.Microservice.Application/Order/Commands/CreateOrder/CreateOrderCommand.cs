using CodeDesignPlus.Net.Generator;
using CodeDesignPlus.Net.Microservice.Application.Order.DataTransferObjects;
using FluentValidation;
using MediatR;

namespace CodeDesignPlus.Net.Microservice.Application.Order.Commands.CreateOrder;
[DtoGenerator]
public class CreateOrderCommand : IRequest
{
    public Guid Id { get; set; }
    public required ClientDto Client { get; set; }
    public Guid Tenant { get; set; }
}
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
