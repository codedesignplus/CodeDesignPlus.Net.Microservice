using CodeDesignPlus.Net.Exceptions;

namespace CodeDesignPlus.Net.Microservice.Application;

public class Errors: IErrorCodes
{
    public static readonly Error OrderNotFound = new("200", "The order does not exist.");
    public static readonly Error OrderAlreadyExists = new("201", "The order already exists.");
    public static readonly Error ClientIsNull = new("300", "The client is null."); 
}
