﻿using CodeDesignPlus.Net.Microservice.Application.Order.DataTransferObjects;
using MediatR;

namespace CodeDesignPlus.Net.Microservice.Application.Order.Commands.AddProductToOrder
{
    public class AddProductToOrderCommand : IRequest
    {
        public Guid Id { get; set; }
        public required ProductDto Product { get; set; }
        public int Quantity { get; set; }
    }
}
