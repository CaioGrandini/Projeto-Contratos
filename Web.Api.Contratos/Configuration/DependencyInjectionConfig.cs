using Web.Api.Contratos.Context;
using Web.Api.Contratos.Services;

namespace Web.Api.Contratos.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<MyDbContext>();
            services.AddScoped<CsvServices>();
            return services;
        }
    }
}
