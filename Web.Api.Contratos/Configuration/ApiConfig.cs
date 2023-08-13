namespace Web.Api.Contratos.Configuration
{
    public static class ApiConfig
    {
        public static IServiceCollection AddWebApiConfig(this IServiceCollection services)
        {
            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy("Development",
                    builder => builder.AllowAnyOrigin()
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());

                options.AddPolicy("Production",
                    builder => builder.WithOrigins("https://localhost:4200") //porta angular - ng serve
                                      .AllowAnyMethod()
                                      .SetIsOriginAllowedToAllowWildcardSubdomains()
                                      .AllowAnyHeader()
                                      .AllowCredentials());

            });
            return services;
        }

        public static IApplicationBuilder UseWebApiConfig(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseCors("Development");
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseCors("Production"); //Cors Production
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            return app;
        }
    }
}
