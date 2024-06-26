﻿using System.Net;
using CodeDesignPlus.Net.Exceptions;
using CodeDesignPlus.Net.Exceptions.Models;

namespace CodeDesignPlus.Net.Microservice.Rest.Core.Middlewares;

public class ExceptionMiddlware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            await HandleExceptionsAsync(context, ex);
        }
        catch (CodeDesignPlusException ex)
        {
            await HandleExceptionsAsync(context, ex);
        }
        catch (Exception ex)
        {
            await HandleExceptionsAsync(context, ex);
        }
    }


    private static Task HandleExceptionsAsync(HttpContext context, CodeDesignPlusException exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        var response = new ErrorResponse(context.TraceIdentifier, exception.Layer);

        response.AddError(exception.Code, exception.Message, null);

        return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }

    private static Task HandleExceptionsAsync(HttpContext context, ValidationException exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        var errors = exception.Errors.Select(e => new ErrorDetail(e.ErrorCode, e.PropertyName, e.ErrorMessage));

        var response = new ErrorResponse(context.TraceIdentifier, Layer.Application);

        response.Errors.AddRange(errors);

        return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }

    private static Task HandleExceptionsAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var result = new ErrorResponse(context.TraceIdentifier, Layer.None);

        result.AddError("0-000", exception.Message, null);

        return context.Response.WriteAsync(JsonConvert.SerializeObject(result));
    }
}