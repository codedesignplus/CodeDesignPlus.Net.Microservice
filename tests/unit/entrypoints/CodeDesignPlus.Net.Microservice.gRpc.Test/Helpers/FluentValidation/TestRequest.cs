namespace CodeDesignPlus.Net.Microservice.gRpc.Test.Helpers.FluentValidation;

public class TestRequest : IBaseRequest { }

public class TestResponse
{
    public string? Property { get; set; }
}