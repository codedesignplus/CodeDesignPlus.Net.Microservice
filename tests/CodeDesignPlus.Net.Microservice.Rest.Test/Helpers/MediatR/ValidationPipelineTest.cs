using CodeDesignPlus.Net.Microservice.Rest.Test.Helpers.FluentValidation;

namespace CodeDesignPlus.Net.Microservice.Rest.Test.Helpers.MediatR;

public class ValidationPipelineTest
{
    private readonly Mock<IValidator<TestRequest>> _validatorMock;
    private readonly ValidationPipeline<TestRequest, TestResponse> _pipeline;
    private readonly Mock<RequestHandlerDelegate<TestResponse>> _nextMock;

    public ValidationPipelineTest()
    {
        _validatorMock = new Mock<IValidator<TestRequest>>();
        _pipeline = new ValidationPipeline<TestRequest, TestResponse>([_validatorMock.Object]);
        _nextMock = new Mock<RequestHandlerDelegate<TestResponse>>();
    }

    [Fact]
    public async Task Handle_ShouldCallNext_WhenValidationPasses()
    {
        // Arrange
        var request = new TestRequest();
        _validatorMock.Setup(v => v.Validate(It.IsAny<ValidationContext<TestRequest>>()))
                      .Returns(new ValidationResult());

        // Act
        _ = await _pipeline.Handle(request, _nextMock.Object, CancellationToken.None);

        // Assert
        _nextMock.Verify(n => n(), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowValidationException_WhenValidationFails()
    {
        // Arrange
        var request = new TestRequest();
        var failures = new List<ValidationFailure> { 
            new("Property", "Error") 
        }
        ;
        _validatorMock.Setup(v => v.Validate(It.IsAny<ValidationContext<TestRequest>>()))
                      .Returns(new ValidationResult(failures));

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _pipeline.Handle(request, _nextMock.Object, CancellationToken.None));
        _nextMock.Verify(n => n(), Times.Never);
    }
}

