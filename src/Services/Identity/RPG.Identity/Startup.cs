using Duende.IdentityServer.EntityFramework.Options;
using Duende.IdentityServer.Services;
using FluentValidation;
using HealthChecks.UI.Client;
using IdentityServer4.AccessTokenValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RPG.BuildingBlocks.Common.Constants;
using RPG.BuildingBlocks.Common.EventBus;
using RPG.BuildingBlocks.Common.Extensions;
using RPG.BuildingBlocks.Common.Middlewares;
using RPG.BuildingBlocks.Common.SeedWork;
using RPG.BuildingBlocks.Common.ServiceDiscovery;
using RPG.BuildingBlocks.Common.Utils;
using RPG.Identity.Domain.UserAggregate;
using RPG.Identity.Infrastructure;
using RPG.Identity.Services;
using RPG.Identity.Utils;
using RPG.RPG.BuildingBlocks.Common.AuthorizationAttributes;
using System.Globalization;
using System.Reflection;

namespace RPG.Identity
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {


            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            string serviceId = Configuration["ServiceId"];
            string authorityUrl = Configuration["AUTHORITY_URL"];
            bool requireHttpsMetadata = bool.Parse(Configuration["AUTHORITY_REQUIRE_HTTPS_METADATA"]);

            //var connectionString = Configuration.GetConnectionString("DefaultConnection");
            //string serviceId = "identity";
            //string authorityUrl = "http://localhost:7001";
            //bool requireHttpsMetadata = false;

            // var connectionString = "Server=127.0.0.1,1433;Database=OurGlassIdentity;User Id=sa;password=Test123!";
            var migrationsAssembly =
                typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services
                .AddLocalization(options =>
                    options.ResourcesPath = "Resources");

            //Only for migrations
            var storeOptions = new ConfigurationStoreOptions();
            services.AddSingleton(storeOptions);
            //Only for migrations end

            services
                .AddControllersWithViews();

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddControllers().AddDapr();

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = authorityUrl;
                options.Audience = serviceId;
                options.RequireHttpsMetadata = requireHttpsMetadata;

                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateAudience = false
                };
            });

            services
                .AddDbContext<ApplicationDbContext>(options =>
                    options
                        .UseSqlServer(connectionString,
                        sqlServerOptionsAction: sqlOptions =>
                        {
                            sqlOptions
                                .MigrationsAssembly(migrationsAssembly);

                            //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency
                            sqlOptions
                                .EnableRetryOnFailure(maxRetryCount: 15,
                                maxRetryDelay: TimeSpan.FromSeconds(30),
                                errorNumbersToAdd: null);
                        }));

            services
                .AddIdentity<ApplicationUser, IdentityRole>(o =>
                {
                    o.Password.RequiredLength = 6;
                    o.Password.RequiredUniqueChars = 0;
                    o.Password.RequireLowercase = false;
                    o.Password.RequireUppercase = false;
                    o.Password.RequireNonAlphanumeric = false;
                    o.Password.RequireDigit = false;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddErrorDescriber<MultilanguageIdentityErrorDescriber>(); ;

            var builder =
                services
                    .AddIdentityServer(x =>
                    {
                        x.InputLengthRestrictions.Scope = 1000;
                    })
                    .AddAspNetIdentity<ApplicationUser>()
                    .AddConfigurationStore(options =>
                    {
                        options.ConfigureDbContext = builder =>
                            builder
                                .UseSqlServer(connectionString,
                                sqlServerOptionsAction: sqlOptions =>
                                {
                                    sqlOptions.MigrationsAssembly(
                                        migrationsAssembly
                                    );

                                    //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency
                                    sqlOptions
                                        .EnableRetryOnFailure(maxRetryCount: 15,
                                        maxRetryDelay: TimeSpan.FromSeconds(30),
                                        errorNumbersToAdd: null);
                                });
                    })
                    .AddOperationalStore(options =>
                    {
                        options.ConfigureDbContext = builder =>
                            builder
                                .UseSqlServer(connectionString,
                                sqlServerOptionsAction: sqlOptions =>
                                {
                                    sqlOptions.MigrationsAssembly(
                                        migrationsAssembly
                                    );

                                    //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency
                                    sqlOptions
                                        .EnableRetryOnFailure(maxRetryCount: 15,
                                        maxRetryDelay: TimeSpan.FromSeconds(30),
                                        errorNumbersToAdd: null);
                                });
                    })
                    .AddCustomTokenRequestValidator<CustomSignInService>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Authorization.SERVICE_DISCOVERY, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", SharedScopes.SCOPE_SERVICE_DISCOVERY);
                });

                options.AddPolicy(Authorization.EVENTBUS, policy =>
                {
                    policy.Requirements.Add(new ValidApiTokenRequirement());
                });
            });

            //if (!(Environment.IsDevelopment() || Environment.IsEnvironment("Testing")))
            //{
            //    //https://damienbod.com/2020/02/10/create-certificates-for-identityserver4-signing-using-net-core/
            //    Console.WriteLine("loading rsaCert.pfx cert v2");
            //    var rsaCertificate =
            //        new X509Certificate2(Path
            //                .Combine(Environment.ContentRootPath,
            //                "cert_rsa512.pfx"),
            //            "1234");
            //    if (rsaCertificate != null)
            //    {
            //        Console.WriteLine("loading rsaCert.pfx cert loaded");
            //    }
            //    builder.AddSigningCredential(rsaCertificate);
            //}

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(s => s.FullName.Replace("+", "."));
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = serviceId,
                    Version = "v1"
                });
                c.SwaggerDoc("v2", new OpenApiInfo
                {
                    Title = serviceId,
                    Version = "V2.0"
                });
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

            services.AddControllersWithViews().AddNewtonsoftJson();

            services.AddScoped<CommonValidator>();
            services.AddScoped<CustomRequestInfo>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());


            services.AddScoped<IServiceDiscovery, ServiceDiscoveryDapr>();
            services.AddScoped<IEventBus, EventBusDapr>();
            services.AddTransient<IClaimsService, CustomClaimsService>();
            services.AddSingleton<IAuthorizationHandler, ValidApiTokenHandler>();

            services.AddOptions();

            if (Environment.IsDevelopment())
            {
                Console.WriteLine("Cors active");
                services.AddCors(options =>
                {
                    options.AddPolicy(Cors.LOCALHOST,
                    builder =>
                    {
                        builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
                });
            }

            services
                .AddSingleton(settings =>
                {
                    CommonGlobalAppSingleSettings commonGlobalAppSingleSettings =
                        new CommonGlobalAppSingleSettings();
                    commonGlobalAppSingleSettings.DbConnectionString =
                        connectionString;
                    return commonGlobalAppSingleSettings;
                });

            services.AddCustomHealthCheck(Configuration, HealthCheckExtension.ServiceType.SQL);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            string serviceId = Configuration["ServiceId"];
            if (Configuration.GetValue<bool>("use_swagger", false))
            {
                app.UseSwagger(options =>
                {
                    options.PreSerializeFilters.Add((swagger, httpReq) =>
                    {
                        if (httpReq.Headers.ContainsKey("X-Forwarded-Host"))
                        {
                            var basePath = serviceId;
                            var serverUrl = $"{httpReq.Headers["X-Forwarded-Proto"]}://{httpReq.Headers["X-Forwarded-Host"]}";
                            swagger.Servers = new List<OpenApiServer> { new OpenApiServer { Url = serverUrl } };
                        }
                    });
                });

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("v1/swagger.json", "Service");
                    c.SwaggerEndpoint("v2/swagger.json", "Service 2.0");
                    c.OAuthClientId(ClientIds.UnrealMobile);
                    c.OAuthAppName("API - Swagger");
                    c.OAuthUsePkce();
                });
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else if (env.IsProduction())
            {
                #region GKE proxy config
                //https://github.com/IdentityServer/IdentityServer4/issues/1331
                var forwardOptions =
                    new ForwardedHeadersOptions
                    {
                        ForwardedHeaders =
                            ForwardedHeaders.XForwardedFor |
                            ForwardedHeaders.XForwardedProto,
                        RequireHeaderSymmetry = false
                    };
                forwardOptions.KnownNetworks.Clear();
                forwardOptions.KnownProxies.Clear();
                app.UseForwardedHeaders(forwardOptions);
                #endregion
            }


            var supportedCultures =
                new[] { new CultureInfo("en"), new CultureInfo("es") };
            var requestLocalizationOptions =
                new RequestLocalizationOptions
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

            if (env.IsDevelopment())
                app.UseCors(Cors.LOCALHOST);

            app.UseCloudEvents();

            app.UseStaticFiles();
            MiddlewareExtensions.UseHttpLoggingExtension(app);
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app
                 .UseEndpoints(endpoints =>
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

        }

    }
}

