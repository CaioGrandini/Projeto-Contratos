
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Web.Api.Contratos.Data;
using Web.Api.Contratos.Extensions;

namespace Web.Api.Contratos.Configuration
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfig (this IServiceCollection services, 
                                                                 IConfiguration configuration)
        {
            
            services.AddDbContext<ApplicationDbContext>(options => 
                options.UseSqlServer(configuration.GetConnectionString("WebApiDatabase")));

            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // JWT
            //estamos pegando a secao do appsettings
            var appSettingsSection = configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            //estamos pegando os dados da classe configurada
            var appSettings = appSettingsSection.Get<AppSettings>();
            //estamos pegando o encoding do secret
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            //adicionamos a autenticacao
            services.AddAuthentication(x =>
            {
                //as opções geram token e validam o token
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {

                //configuração das informações contidas no token
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    //configuracao da chave para uma criptografia
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = appSettings.ValidoEm,
                    ValidIssuer = appSettings.Emissor
                };
            });

            return services;
        }
    }
}
