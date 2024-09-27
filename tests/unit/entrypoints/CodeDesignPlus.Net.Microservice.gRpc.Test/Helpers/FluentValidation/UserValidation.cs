namespace CodeDesignPlus.Net.Microservice.gRpc.Test.Helpers.FluentValidation;

public class UserValidation : AbstractValidator<UserModel>
{
    public UserValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
    }
}
