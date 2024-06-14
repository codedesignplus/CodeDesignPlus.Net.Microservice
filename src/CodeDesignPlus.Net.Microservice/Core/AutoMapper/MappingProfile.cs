using AutoMapper;
using CodeDesignPlus.Net.Microservice.Application.Order.Commands.AddProductToOrder;
using CodeDesignPlus.Net.Microservice.Application.Order.Commands.CancelOrder;
using CodeDesignPlus.Net.Microservice.Application.Order.Commands.CompleteOrder;
using CodeDesignPlus.Net.Microservice.Application.Order.Commands.CreateOrder;
using CodeDesignPlus.Net.Microservice.Application.Order.Commands.RemoveProduct;
using CodeDesignPlus.Net.Microservice.Application.Order.Commands.UpdateQuantityProduct;
using CodeDesignPlus.Microservice.Api.Dtos;
using CodeDesignPlus.Net.Microservice.Domain;
using CodeDesignPlus.Net.Microservice.Application.Order.DataTransferObjects;
using CodeDesignPlus.Net.Microservice.Domain.Entities;


namespace CodeDesignPlus.Net.Microservice.Core.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AddProductToOrderDto, AddProductToOrderCommand>();
            CreateMap<CancelOrderDto, CancelOrderCommand>();
            CreateMap<CompleteOrderDto, CompleteOrderCommand>();
            CreateMap<CreateOrderDto, CreateOrderCommand>();
            CreateMap<RemoveProductDto, RemoveProductCommand>();
            CreateMap<UpdateQuantityProductDto, UpdateQuantityProductCommand>();

            CreateMap<OrderAggregate, OrderDto>();
            CreateMap<ClientEntity, ClientDto>();
            CreateMap<ProductEntity, ProductDto>();
        }
    }

}
