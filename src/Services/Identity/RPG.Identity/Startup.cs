using FluentValidation;
using Google.Api;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.OpenApi.Models;
using RPG.BuildingBlocks.Common.Constants;
using RPG.BuildingBlocks.Common.EventBus;
using RPG.BuildingBlocks.Common.Middlewares;
using RPG.BuildingBlocks.Common.ServiceDiscovery;
using RPG.BuildingBlocks.Middlewares;
using RPG.BuildingBlocks.Utils;
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

        public virtual void ConfigureServices(IServiceCollection services)
        {
            //string serviceId = "identity";
            //string authorityUrl = "http://localhost:7001";
            //bool requireHttpsMetadata = false;
            //string connectionString = Configuration[$"{serviceId}_db"];

            string serviceId = Configuration["ServiceId"];
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            string authorityUrl = Configuration["AUTHORITY_URL"];
            bool requireHttpsMetadata = bool.Parse(Configuration["AUTHORITY_REQUIRE_HTTPS_METADATA"]);
            Console.WriteLine($"Service Id {serviceId}");
            Console.WriteLine($"authority URL {authorityUrl}");
            Console.WriteLine($"requireHttpsMetadata {requireHttpsMetadata}");


            services.AddControllers();//.AddDapr();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
            services.AddValidatorsFromAssembly(typeof(Program).Assembly);
            services.AddAutoMapper(typeof(Program).Assembly);

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("es"), // Idioma espa�ol
                    new CultureInfo("en")  // Idioma ingl�s
                };

                options.DefaultRequestCulture = new RequestCulture("es");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            // Registra los archivos de recursos de "BuildingBlocks"
            services.AddTransient<IStringLocalizerFactory, ResourceManagerStringLocalizerFactory>();
            services.AddTransient(typeof(IStringLocalizer), typeof(StringLocalizer<CommonResources>));

            // Configura FluentValidation

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Nombre de tu API", Version = "v1" });
            });



            // Configuraci�n del DbContext para Identity Server

            var migrationsAssembly =
                typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            services
                .AddDbContext<UserIdentityDbContext>(options =>
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
                .AddEntityFrameworkStores<UserIdentityDbContext>()
                .AddDefaultTokenProviders();

                //.AddErrorDescriber<MultilanguageIdentityErrorDescriber>(); ;
            services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();

            services.AddIdentityServer()
                    .AddConfigurationStore(options =>
                    {
                        options.ConfigureDbContext = b => b.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
                    })
                    .AddOperationalStore(options =>
                    {
                        options.ConfigureDbContext = b => b.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
                    })
            .AddDeveloperSigningCredential()
            .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
            .AddCustomTokenRequestValidator<CustomSignInService>()
            .AddCorsPolicyService<CustomCorsPolicyService>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<AuthorizationPropagationMiddleware>();

            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Nombre de tu API v1");
            });
            app.UseRouting();
            app.UseIdentityServer();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); 
            });
            app.IdentityDatabase();

        }

    }
}

