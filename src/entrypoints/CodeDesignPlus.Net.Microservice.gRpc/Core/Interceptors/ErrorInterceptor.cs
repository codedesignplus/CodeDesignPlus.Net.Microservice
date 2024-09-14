using CodeDesignPlus.Net.Exceptions.Models;
using Grpc.Core.Interceptors;

namespace CodeDesignPlus.Net.Microservice.gRpc.Core.Interceptors;

public class ErrorInterceptor : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (Exception ex)
        {
            throw HandleException(ex);
        }
    }

    public override async Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(
        IAsyncStreamReader<TRequest> requestStream,
        ServerCallContext context,
        ClientStreamingServerMethod<TRequest, TResponse> continuation)
    {

        try
        {
            return await continuation(requestStream, context);
        }
        catch (Exception ex)
        {
            throw HandleException(ex);
        }
    }

    public override async Task ServerStreamingServerHandler<TRequest, TResponse>(
        TRequest request,
        IServerStreamWriter<TResponse> responseStream,
        ServerCallContext context,
        ServerStreamingServerMethod<TRequest, TResponse> continuation)
    {
        
        try
        {
            await continuation(request, responseStream, context);
        }
        catch (Exception ex)
        {
            throw HandleException(ex);
        }
    }

    public override async Task DuplexStreamingServerHandler<TRequest, TResponse>(
        IAsyncStreamReader<TRequest> requestStream,
        IServerStreamWriter<TResponse> responseStream,
        ServerCallContext context,
        DuplexStreamingServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            await continuation(requestStream, responseStream, context);
        }
        catch (Exception ex)
        {
            throw HandleException(ex);
        }
    }

    private static RpcException HandleException(Exception exception)
    {
        return exception switch
        {
            ValidationException ex => HandleValidationException(ex),
            CodeDesignPlusException ex => HandleCodeDesignPlusException(ex),
            _ => HandleGeneralException(exception),
        };
    }

    private static RpcException HandleValidationException(ValidationException exception)
    {
        var errors = exception.Errors.Select(e => new ErrorDetail
        (
            e.ErrorCode,
            e.PropertyName,
            e.ErrorMessage
        )).ToList();

        var response = new ErrorResponse(null, Layer.Application);

        response.Errors.AddRange(errors);

        var metadata = new Metadata
        {
            { "Layer", Layer.Application.ToString() }
        };

        foreach (var error in errors)
        {
            metadata.Add("ValidationError", $"Code: {error.Code}, Property: {error.Field}, Message: {error.Message}");
        }

        var status = new Status(StatusCode.InvalidArgument, "Validation failed");

        return new RpcException(status, metadata, response.ToString());
    }

    private static RpcException HandleCodeDesignPlusException(CodeDesignPlusException exception)
    {
        var response = new ErrorResponse(null, exception.Layer);

        response.Errors.Add(new ErrorDetail(exception.Code, null, exception.Message));

        var metadata = new Metadata
        {
            { "Layer", exception.Layer.ToString() },
            { "Code", exception.Code },
            { "Message", exception.Message }
        };

        var status = new Status(StatusCode.FailedPrecondition, exception.Message);

        return new RpcException(status, metadata, response.ToString());
    }

    private static RpcException HandleGeneralException(Exception exception)
    {
        var response = new ErrorResponse(null, Layer.None);

        response.Errors.Add(new ErrorDetail("0-000", null, exception.Message));

        var metadata = new Metadata
        {
            { "Layer", Layer.None.ToString() },
            { "Code", "0-000" },
            { "Message", exception.Message }
        };

        var status = new Status(StatusCode.Internal, "Internal server error");

        return new RpcException(status, metadata, response.ToString());
    }
}
