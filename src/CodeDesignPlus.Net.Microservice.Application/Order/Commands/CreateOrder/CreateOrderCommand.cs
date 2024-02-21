using CodeDesignPlus.Net.Microservice.Application.Order.DataTransferObjects;
using MediatR;

namespace CodeDesignPlus.Net.Microservice.Application.Order.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest
    {
        public Guid Id { get; set; }
        public required ClientDto Client { get; set; }
        public Guid Tenant { get; set; }
    }
}
