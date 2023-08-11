using Web.Api.Contratos.Context;
using Web.Api.Contratos.Extensions;
using Web.Api.Contratos.Interfaces;
using Web.Api.Contratos.Notificacoes;
using Web.Api.Contratos.Services;

namespace Web.Api.Contratos.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<MyDbContext>();
            services.AddScoped<CsvServices>();
            services.AddScoped<INotificador, Notificador>();
            services.AddScoped<IUser, AspNetUser>();

            return services;
        }
    }
}
