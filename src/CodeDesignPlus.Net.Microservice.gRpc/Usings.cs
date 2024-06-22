global using MediatR;
global using FluentValidation;
global using Mapster;
global using Grpc.Core;
global using MapsterMapper;
global using Google.Protobuf.WellKnownTypes;


global using CodeDesignPlus.Net.Microservice.Application.Order.Commands.AddProductToOrder;
global using CodeDesignPlus.Net.Microservice.Application.Order.Commands.CancelOrder;
global using CodeDesignPlus.Net.Microservice.Application.Order.Commands.CompleteOrder;
global using CodeDesignPlus.Net.Microservice.Application.Order.Commands.CreateOrder;
global using CodeDesignPlus.Net.Microservice.Application.Order.Commands.RemoveProduct;
global using CodeDesignPlus.Net.Microservice.Application.Order.Commands.UpdateQuantityProduct;
global using CodeDesignPlus.Net.Microservice.Application.Order.Queries.FindOrderById;
global using CodeDesignPlus.Net.Microservice.Application.Order.Queries.GetAllOrders;
global using CodeDesignPlus.Net.Microservice.Application.Order.DataTransferObjects;
global using CodeDesignPlus.Net.Exceptions;