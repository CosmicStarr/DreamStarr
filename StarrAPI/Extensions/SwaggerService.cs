using Microsoft.AspNetCore.Builder;

namespace StarrAPI.Extensions
{
    public static class SwaggerService
    {
        public static IApplicationBuilder SwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "StarrAPI v1"));

            return app;
        }
    }
}