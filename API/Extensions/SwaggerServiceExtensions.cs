using Microsoft.OpenApi.Models;

namespace API.Extensions
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection servicesCollection)
        {
            servicesCollection.AddSwaggerGen(opt =>
                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "SNAP",
                    Version = "v1",
                }));
            return servicesCollection;
        }

        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(opt =>
                opt.SwaggerEndpoint("/swagger/v1/swagger.json", "SNAP API v1"));
            return app;
        }
    }
}
