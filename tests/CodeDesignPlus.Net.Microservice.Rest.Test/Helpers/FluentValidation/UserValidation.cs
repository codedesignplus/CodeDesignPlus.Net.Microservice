using System;
using CodeDesignPlus.Net.Microservice.Rest.Test.Helpers.Models;
using FluentValidation;

namespace CodeDesignPlus.Net.Microservice.Rest.Test.Helpers.FluentValidation;

public class UserValidation: AbstractValidator<UserModel>
{
    public UserValidation()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
    }
}
