using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RPG.Identity.Infrastructure;
using System.Globalization;
using RPG.BuildingBlocks.Utils;
using Microsoft.AspNetCore.Identity;
using RPG.Identity.Domain.UserAggregate;
using RPG.Identity.Utils;
using RPG.BuildingBlocks.Common.Middlewares;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using RPG.Identity.Services;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Reflection;
using Google.Api;

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
        public void ConfigureServicesTemp(IServiceCollection services)
        {

            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            string serviceId = Configuration["ServiceId"];
            string authorityUrl = Configuration["AUTHORITY_URL"];
            bool requireHttpsMetadata = bool.Parse(Configuration["AUTHORITY_REQUIRE_HTTPS_METADATA"]);
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
            services.AddValidatorsFromAssembly(typeof(Program).Assembly);
            services.AddAutoMapper(typeof(Program).Assembly);


            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("es"), // Idioma español
                    new CultureInfo("en")  // Idioma inglés
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



            // Configuración del DbContext para Identity Server
            var connectionString = Configuration.GetConnectionString("DefaultConnection"); // Reemplaza esto con la cadena de conexión de tu base de datos

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
                .AddErrorDescriber<MultilanguageIdentityErrorDescriber>(); ;

            services.AddIdentityServer()
                    .AddConfigurationStore(options =>
                    {
                        options.ConfigureDbContext = b => b.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
                    })
                    .AddOperationalStore(options =>
                    {
                        options.ConfigureDbContext = b => b.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
                        options.EnableTokenCleanup = true;
                    })
            .AddCustomTokenRequestValidator<CustomSignInService>();

            services.AddAuthentication()
                .AddLocalApi("Bearer", option =>
                {
                    option.ExpectedScope = "character";
                });

            services.AddAuthorization();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Nombre de tu API v1");
            });
            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); 
            });
            app.IdentityDatabase();

        }

    }
}

