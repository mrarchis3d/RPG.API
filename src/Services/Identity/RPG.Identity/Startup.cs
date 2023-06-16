using FluentValidation;
using Google.Api;
using IdentityServer4.AccessTokenValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using Microsoft.OpenApi.Models;
using MongoDB.Driver.Core.Configuration;
using RPG.BuildingBlocks.Common.Constants;
using RPG.BuildingBlocks.Common.Extensions;
using RPG.BuildingBlocks.Common.Middlewares;
using RPG.BuildingBlocks.Common.Utils;
using RPG.Identity.Domain.UserAggregate;
using RPG.Identity.Infrastructure;
using RPG.Identity.Services;
using RPG.Identity.Utils;
using System.Globalization;
using System.Net.NetworkInformation;
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

            string serviceId = Configuration["ServiceId"]!;
            string authorityUrl = Configuration["AUTHORITY_URL"]!;
            bool requireHttpsMetadata = bool.Parse(Configuration["AUTHORITY_REQUIRE_HTTPS_METADATA"]!);

            services.AddControllersWithViews().AddNewtonsoftJson();
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddEndpointsApiExplorer();
            services.AddHttpContextAccessor();
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
            services.AddApiVersioning(
            options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });

            services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(s => s.FullName.Replace("+", "."));
                foreach (var version in SwaggerVersioning.Versions)
                {
                    c.SwaggerDoc($"v{version}", new OpenApiInfo
                    {
                        Title = "identity",
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
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            var migrationsAssembly =
                typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
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

            services.AddMediatR(cfg =>
                    cfg.RegisterServicesFromAssembly(typeof(Startup).Assembly));
            services.AddScoped<CommonValidator>();
            services.AddScoped<CustomRequestInfo>();

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
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
            // Otras configuraciones adicionales

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseEndpoints(endpoint => endpoint.MapControllers());

            if (env.IsDevelopment())
            {

                // Habilita la generaci�n del documento JSON de Swagger
                app.UseSwagger();

                // Especifica la ruta para acceder al documento JSON de Swagger
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "RPG.API");
                });
                app.UseDeveloperExceptionPage();

            }
            if (env.IsDevelopment())
                app.UseCors(Cors.LOCALHOST);
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

            app.UseStaticFiles();
            MiddlewareExtensions.UseHttpLogging(app);
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseMiddleware<ExceptionHandlingMiddleware>();

        }

    }
}

