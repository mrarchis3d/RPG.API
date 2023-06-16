using IdentityModel;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver.Core.Configuration;
using Newtonsoft.Json;
using RPG.BuildingBlocks.Common.Constants;
using RPG.BuildingBlocks.Common.Extensions;
using RPG.BuildingBlocks.Common.Providers.Identity;
using RPG.RPG.BuildingBlocks.Common.AuthorizationAttributes;
using Serilog;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using RPG.BuildingBlocks.Common.Middlewares;
using System.Globalization;
using RPG.BuildingBlocks.Common.Utils;
using Microsoft.OpenApi.Models;
using Google.Api;

namespace RPG.BuildingBlocks.Common
{
    public static class CommonExtensions
    {

        public static IServiceCollection AddCommonServices(this IServiceCollection services, IConfiguration configuration)
        {

            string serviceId = configuration["ServiceId"];
            string authorityUrl = configuration["AUTHORITY_URL"];
            bool requireHttpsMetadata = bool.Parse(configuration["AUTHORITY_REQUIRE_HTTPS_METADATA"]);

            Console.WriteLine($"Service Id {serviceId}");
            Console.WriteLine($"authority URL {authorityUrl}");
            Console.WriteLine($"requireHttpsMetadata {requireHttpsMetadata}");

            services.AddHttpContextAccessor();
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddScoped<CommonValidator>();
            services.AddScoped<CustomRequestInfo>();

            services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(s => s.FullName.Replace("+", "."));
                foreach (var version in SwaggerVersioning.Versions)
                {
                    c.SwaggerDoc($"v{version}", new OpenApiInfo
                    {
                        Title = serviceId,
                        Version = $"V{version}.0"
                    });
                }

                c.OperationFilter<SwaggerDeprecationFilter>();
                c.OperationFilter<AuthorizeCheckOperationFilter>();
                c.DocumentFilter<RemoveInternalEndpoints>();
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Password = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(authorityUrl + "/connect/authorize"),
                            TokenUrl = new Uri(authorityUrl + "/connect/token"),
                            Scopes = Scopes.ApiScopes
                        }
                    }
                });
            });

            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                o.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                });

            services.AddControllers().AddNewtonsoftJson(x =>
            {
                x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                x.SerializerSettings.Formatting = Formatting.Indented;
            });
            services.AddHttpClient<IIdentityProvider, IdentityProvider>((serv, opt) =>
            {
                var accessor = serv.GetRequiredService<IHttpContextAccessor>();

                if (accessor.HttpContext is null)
                    return;

                if (accessor.HttpContext.User.Claims.Any(x => x.Type == JwtClaimTypes.ClientId && x.Value == ClientIds.Internal))
                    opt.DefaultRequestHeaders.Add("X-USER-ID",
                        accessor.HttpContext.GetHeaderUserId().Result);

                opt.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", accessor.HttpContext.GetBearerToken().Result);
            });
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = authorityUrl;
                options.Audience = serviceId;
                options.RequireHttpsMetadata = requireHttpsMetadata;

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        // If the request is for our hub...
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.HasValue && path.Value.Contains("/messaging"))
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Default", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireAssertion(ctx =>
                    {
                        return ctx.User.HasClaim(claim => claim.Type == UserConstants.ACTIVE_CLAIM && claim.Value == "True")
                            || ctx.User.HasClaim(claim =>
                                claim.Type == JwtClaimTypes.ClientId && claim.Value == Constants.ClientIds.Internal);
                    });
                });

                options.AddPolicy(Authorization.INCREATION_USER, policy => policy.RequireAuthenticatedUser());

                options.AddPolicy(Authorization.SERVICE_DISCOVERY, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", SharedScopes.SCOPE_SERVICE_DISCOVERY);
                });

                options.AddPolicy(Authorization.EVENTBUS, policy =>
                {
                    policy.Requirements.Add(new ValidApiTokenRequirement());
                });

                options.DefaultPolicy = options.GetPolicy("Default")!;
            });
            return services;
        }


        public static IApplicationBuilder UseAppCommonServices(
            this IApplicationBuilder app,
            IConfiguration configuration,
            bool useLogging = false
        )
        {
            //string serviceId = configuration["ServiceId"];
            if (configuration.GetValue<bool>("use_swagger", false))
            {
                app.UseSwagger(options =>
                {
                    options.PreSerializeFilters.Add((swagger, httpReq) =>
                    {
                        if (httpReq.Headers.ContainsKey("X-Forwarded-Host"))
                        {
                            var basePath = "character";
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
                    c.OAuthClientId(ClientIds.Flutter);
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

            app.UseCloudEvents();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            //app.UseMiddleware<UseCacheMiddleware>();

            app.UseEndpoints(endpoints =>
            {

                endpoints.MapSubscribeHandler();
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
            });

            return app;
        }
    }
}
