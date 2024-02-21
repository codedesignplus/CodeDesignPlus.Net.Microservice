using CodeDesignPlus.Net.Core.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CodeDesignPlus.Net.Microservice.Infrastructure
{
    public class Startup : IStartupServices
    {
        public void Initialize(IServiceCollection services, IConfiguration configuration)
        {
            //services.AddSingleton<IOrderRepository, OrderRepository>();
        }
    }
}
