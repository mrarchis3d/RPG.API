using RPG.BuildingBlocks.Common.SeedWork;
using RPG.BuildingBlocks.Common.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using RPG.BuildingBlocks.Common.EventBus;
using RPG.BuildingBlocks.Common.ServiceDiscovery;
using RPG.BuildingBlocks.Common.Email;
using RPG.BuildingBlocks.Common.SMS;
using IdentityServer4.AccessTokenValidation;
using Microsoft.IdentityModel.Logging;
using Microsoft.AspNetCore.Hosting;
using RPG.BuildingBlocks.Common.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using RPG.BuildingBlocks.Common.CommonLog;
using RPG.BuildingBlocks.Common.Providers.Identity;
using FluentValidation;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using RPG.BuildingBlocks.Common.StateManagement;
using RPG.RPG.BuildingBlocks.Common.AuthorizationAttributes;
using RPG.BuildingBlocks.Utils;

namespace RPG.BuildingBlocks.Common.Extensions
{
    public static class ServiceCommonExtension
    {
        public static IServiceCollection AddAppCommonDependencies<TDbContext, TMediatrCommandHandler>(
            this IServiceCollection services,
            IConfiguration configuration,
            IWebHostEnvironment Environment,
            IEnumerable<Assembly> additionalAssemblies = null)
            where TDbContext : DbContext
        {
            string secretStoreSectionName = configuration["ServiceId"];
            string dbConnectionString = configuration[$"{secretStoreSectionName}_db"];

            bool isTestingEnv = Environment.EnvironmentName == "Testing";

            CommonDependencies<TMediatrCommandHandler>(services, configuration, dbConnectionString, isTestingEnv, additionalAssemblies);
            services.AddCustomHealthCheck(configuration, HealthCheckExtension.ServiceType.SQL);

            services
                .AddDbContextPool<TDbContext>(options =>
                {

                    options.UseSqlServer(dbConnectionString,
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                    });
                    if (isTestingEnv)
                    {
                        options.EnableSensitiveDataLogging();
                        options.LogTo(Log.Logger.Information, LogLevel.Information, DbContextLoggerOptions.UtcTime);
                    }
                },
                    10 //Connection pool size
                       //,ServiceLifetime.Scoped
                );

            return services;
        }

        public static IServiceCollection AddAppCommonDependenciesAggregate<TMediatrCommandHandler>(
            this IServiceCollection services,
            IConfiguration configuration,
            IWebHostEnvironment Environment
        )
        {
            bool isTestingEnv = Environment.EnvironmentName == "Testing";

            CommonDependencies<TMediatrCommandHandler>(services, configuration, string.Empty, isTestingEnv);
            services.AddCustomHealthCheck(configuration, HealthCheckExtension.ServiceType.AGGREGATOR);

            return services;
        }
  

        private static void CommonDependencies<TMediatrCommandHandler>(
            this IServiceCollection services,
            IConfiguration configuration,
            string dbConnectionString,
            bool isTestingEnv,
            IEnumerable<Assembly> additionalAssemblies = null)
        {
            IdentityModelEventSource.ShowPII = true;
            
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            string serviceId = configuration["ServiceId"];
            string authorityUrl = configuration["AUTHORITY_URL"];
            bool requireHttpsMetadata = bool.Parse(configuration["AUTHORITY_REQUIRE_HTTPS_METADATA"]);

            Console.WriteLine($"Service Id {serviceId}");
            Console.WriteLine($"authority URL {authorityUrl}");
            Console.WriteLine($"requireHttpsMetadata {requireHttpsMetadata}");

            //http://docs.identityserver.io/en/release/topics/apis.html
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
                
                //c.OperationFilter<SwaggerDeprecationFilter>();
                //c.OperationFilter<AuthorizeCheckOperationFilter>();
                //c.DocumentFilter<RemoveInternalEndpoints>();
                //c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                //{
                //    Type = SecuritySchemeType.OAuth2,
                //    Flows = new OpenApiOAuthFlows
                //    {
                //        Password = new OpenApiOAuthFlow
                //        {
                //            AuthorizationUrl = new Uri(authorityUrl + "/connect/authorize"),
                //            TokenUrl = new Uri(authorityUrl + "/connect/token"),
                //            Scopes = Scopes.ApiScopes
                //        }
                //    }
                //});        
            });
            
            //services.AddApiVersioning(o =>
            //{
            //    o.AssumeDefaultVersionWhenUnspecified = true;
            //    o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
            //    o.ReportApiVersions = true;
            //});
            
            //services.AddVersionedApiExplorer(
            //    options =>
            //    {
            //        options.GroupNameFormat = "'v'VVV";
            //        options.SubstituteApiVersionInUrl = true;
            //    });

            services.AddMediatR(typeof(TMediatrCommandHandler).Assembly, typeof(CommonResources).Assembly);

            if (additionalAssemblies is not null)
            {
                foreach (var assembly in additionalAssemblies)
                {
                    services.AddMediatR(assembly);
                }
            }
            
            services.AddValidatorsFromAssemblyContaining<TMediatrCommandHandler>();
            services.AddControllers().AddNewtonsoftJson(x =>
            {
                x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                x.SerializerSettings.Formatting = Formatting.Indented;
            }).AddDapr();

            services.AddSingleton(settings =>
            {
                CommonGlobalAppSingleSettings commonGlobalAppSingleSettings = new CommonGlobalAppSingleSettings();
                commonGlobalAppSingleSettings.DbConnectionString = dbConnectionString;
                return commonGlobalAppSingleSettings;
            });

            services.AddScoped<CentralizedLogger>();

            services.AddScoped<CommonValidator>();
            services.AddScoped<CustomRequestInfo>();
            services.AddSingleton<IAuthorizationHandler, ValidApiTokenHandler>();
            services.AddHttpContextAccessor();
            services.AddAutoMapper(typeof(TMediatrCommandHandler).Assembly);

            services.AddHttpClient<IIdentityProvider, IdentityProvider>((serv ,opt) =>
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

            if (isTestingEnv)
            {
                //services.AddScoped<IEventBus, EventBusMock>();
                //services.AddScoped<IServiceDiscovery, ServiceDiscoveryMock>();
                //services.AddScoped<IEmail, EmailMock>();
                //services.AddScoped<ISms, SmsMock>();
            }
            else
            {
                //services.AddScoped<IEventBus, EventBusDapr>();
                services.AddScoped<IServiceDiscovery, ServiceDiscoveryDapr>();
                services.AddScoped<IStateManagement, StateManagementDapr>();
                //services.AddScoped<IEmail, EmailDapr>();
                //services.AddScoped<ISms, SmsTwilio>();
            }
        }
    }
}

