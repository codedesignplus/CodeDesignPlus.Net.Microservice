using AutoMapper;

namespace CodeDesignPlus.Net.Microservice.Core.AutoMapper
{
    public static class AutoMapperExtension
    {
        public static IServiceCollection AddAutoMapperr(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));

            return services;
        }

        public static void CheckAutoMapper(this IApplicationBuilder app)
        {
            var mapper = app.ApplicationServices.GetRequiredService<IMapper>();

            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }

}
