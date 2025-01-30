namespace CodeDesignPlus.Net.Microservice.Domain;

public class Errors: IErrorCodes
{
    public const string IdOrderIsInvalid = "100 : Id is invalid.";
    public const string TenantIsInvalid = "101 : Tenant is invalid.";

    public const string ClientIsNull = "102 : Client is null.";
    public const string IdClientIsInvalid = "103 : Id client is invalid.";
    public const string NameClientIsInvalid = "104 : Name client is invalid.";

    public const string ProductIsNull = "105 : Product is null.";
    public const string IdProductIsInvalid = "106 : Id product is invalid.";
    public const string NameProductIsInvalid = "107 : Name product is invalid.";
    public const string PriceProductIsInvalid = "108 : Price product is invalid.";
    public const string QuantityProductIsInvalid = "109 : Quantity product is invalid.";

    public const string ProductNotFound = "110 : Product not found in the order.";
    public const string OrderAlreadyCompleted = "111 : Order already completed.";
    public const string OrderAlreadyCancelled = "112 : Order already cancelled.";
    public const string DocumentIsNull = "113 : Document is null.";
    public const string TypeDocumentIsNull = "114 : Type document is null.";
    public const string CountryIsNull = "115 : Country is null.";
    public const string StateIsNull = "116 : State is null.";
    public const string CityIsNull = "117 : City is null.";
    public const string AddressIsNull = "118 : Address is null.";
    public const string CodePostalIsInvalid = "119 : Code postal is invalid.";
}
