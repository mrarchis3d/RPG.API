using RPG.BuildingBlocks.Common.Middlewares;
using RPG.BuildingBlocks.Common.Middlewares.Cache;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using RPG.BuildingBlocks.Common.Constants;
using System.Collections.Generic;
using Microsoft.OpenApi.Models;

namespace RPG.BuildingBlocks.Common.Extensions
{
    public static class AppInternationalizationExtension
    {
        public static IApplicationBuilder UseAppCommonServices(
            this IApplicationBuilder app,
            IConfiguration configuration, 
            bool useLogging = false
        )
        {
            string serviceId = configuration["ServiceId"];
            if(configuration.GetValue<bool>("use_swagger", false))
            {
                app.UseSwagger(options =>
                {
                    options.PreSerializeFilters.Add((swagger, httpReq) =>
                    {
                        if (httpReq.Headers.ContainsKey("X-Forwarded-Host"))
                        {                    
                            var basePath = serviceId;
                            var serverUrl = $"{httpReq.Headers["X-Forwarded-Proto"]}://{httpReq.Headers["X-Forwarded-Host"]}/{basePath}";
                            swagger.Servers = new List<OpenApiServer> { new OpenApiServer { Url = serverUrl } };
                        }
                    });
                });
                app.UseSwaggerUI(c => 
                { 
                    foreach (var version in SwaggerVersioning.Versions)
                    {
                        c.SwaggerEndpoint($"v{version}/swagger.json", $"Service {version}.0");
                    }
                    c.OAuthClientId(ClientIds.UnrealDesktop);
                    c.OAuthAppName("API - Swagger");
                    c.OAuthUsePkce();    
                });
            }

            var supportedCultures = new[] { new CultureInfo("en"), new CultureInfo("es") };
            var requestLocalizationOptions = new RequestLocalizationOptions
            {
                //English language is set by default
                DefaultRequestCulture = new RequestCulture("en"),

                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,

                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            };

            app.UseRequestLocalization(requestLocalizationOptions);

            app.UseMiddleware<CustomRequestHandlingMiddleware>();

            app.UseRouting();

            if (useLogging)
                app.UseHttpLogging();

            app.UseCloudEvents();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            //app.UseMiddleware<UseCacheMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/hc", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

                endpoints.MapHealthChecks("/liveness", new HealthCheckOptions
                {
                    Predicate = r => r.Name.Contains("self")
                });

                endpoints.MapSubscribeHandler();
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
            });

            return app;
        }
    }
}

