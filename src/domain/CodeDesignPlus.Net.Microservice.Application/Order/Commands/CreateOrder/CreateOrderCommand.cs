﻿namespace CodeDesignPlus.Net.Microservice.Application.Order.Commands.CreateOrder;

[DtoGenerator]
public record CreateOrderCommand(Guid Id, ClientDto Client, AddressDto Address) : IRequest;

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
                RuleFor(x => x.Client.Document).NotEmpty().NotNull();
                RuleFor(x => x.Client.TypeDocument).NotEmpty().NotNull();
            });

        RuleFor(x => x.Address)
            .NotNull()
            .DependentRules(() =>
            {
                RuleFor(x => x.Address.Country).NotEmpty().NotNull();
                RuleFor(x => x.Address.State).NotEmpty().NotNull();
                RuleFor(x => x.Address.City).NotEmpty().NotNull();
                RuleFor(x => x.Address.Address).NotEmpty().NotNull();
                RuleFor(x => x.Address.CodePostal).NotEmpty().NotNull();
            });
    }
}
