using CodeDesignPlus.Net.Core.Abstractions;

namespace CodeDesignPlus.Net.Microservice.Application;

public class Errors: IErrorCodes
{
    public const string OrderNotFound = "200 : The order does not exist.";
    public const string OrderAlreadyExists = "201 : The order already exists.";
    public const string ClientIsNull = "300 : The client is null."; 
}
