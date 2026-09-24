using CodeDesignPlus.Net.Exceptions;

namespace CodeDesignPlus.Net.Microservice.Infrastructure;

public class Errors: IErrorCodes
{
    public static readonly Error UnknownError = new("000", "An unknown error occurred.");
}
