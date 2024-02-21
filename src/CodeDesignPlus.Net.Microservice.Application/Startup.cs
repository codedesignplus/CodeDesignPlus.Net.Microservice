using CodeDesignPlus.Net.Core.Abstractions;
using CodeDesignPlus.Net.Microservice.Application.Order.DataTransferObjects;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeDesignPlus.Net.Microservice.Application
{
    public class Startup : IStartupServices
    {
        public void Initialize(IServiceCollection services, IConfiguration configuration)
        {
            MapsterConfig.Configure();
        }
    }
}
