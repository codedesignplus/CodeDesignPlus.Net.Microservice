namespace CodeDesignPlus.Net.Microservice.Rest.Test.Helpers.Middlewares;

using System.Net;
using System.Threading.Tasks;
using CodeDesignPlus.Net.Exceptions;
using CodeDesignPlus.Net.Exceptions.Models;
using CodeDesignPlus.Net.Microservice.Rest.Core.Middlewares;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

public class ExceptionMiddlwareTest
{

    [Fact]
    public async Task InvokeAsync_NoException_ReturnsOk()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var mockRequestDelegate = new Mock<RequestDelegate>();
        var middleware = new ExceptionMiddlware(mockRequestDelegate.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.Equal((int)HttpStatusCode.OK, context.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_ValidationException_ReturnsBadRequest()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var mockRequestDelegate = new Mock<RequestDelegate>();
        mockRequestDelegate.Setup(rd => rd(It.IsAny<HttpContext>())).ThrowsAsync(new ValidationException("Validation error", []));
        var middleware = new ExceptionMiddlware(mockRequestDelegate.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.Equal((int)HttpStatusCode.BadRequest, context.Response.StatusCode);
        Assert.Equal("application/json", context.Response.ContentType);
    }

    [Fact]
    public async Task InvokeAsync_CodeDesignPlusException_ReturnsBadRequest()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var mockRequestDelegate = new Mock<RequestDelegate>();
        mockRequestDelegate.Setup(rd => rd(It.IsAny<HttpContext>())).ThrowsAsync(new CodeDesignPlusException(Layer.Application, "CodeDesignPlus error", "1-001"));
        var middleware = new ExceptionMiddlware(mockRequestDelegate.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.Equal((int)HttpStatusCode.BadRequest, context.Response.StatusCode);
        Assert.Equal("application/json", context.Response.ContentType);
    }

    [Fact]
    public async Task InvokeAsync_Exception_ReturnsInternalServerError()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var mockRequestDelegate = new Mock<RequestDelegate>();
        mockRequestDelegate.Setup(rd => rd(It.IsAny<HttpContext>())).ThrowsAsync(new Exception("General error"));
        var middleware = new ExceptionMiddlware(mockRequestDelegate.Object);

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
        Assert.Equal("application/json", context.Response.ContentType);
    }
}

