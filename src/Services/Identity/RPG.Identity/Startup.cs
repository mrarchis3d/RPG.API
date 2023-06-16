using Microsoft.EntityFrameworkCore;
using RPG.Identity.Infrastructure;

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

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            // Configuración del DbContext para Identity Server
            var connectionString = Configuration.GetConnectionString("DefaultConnection"); // Reemplaza esto con la cadena de conexión de tu base de datos
            services.AddDbContext<UserIdentityDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Configuración de Identity Server con el almacenamiento en la base de datos
            services.AddIdentityServer()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder => builder.UseSqlServer(connectionString);
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder => builder.UseSqlServer(connectionString);
                })
                .AddDeveloperSigningCredential();

            // Configuración de Identity Server
            services.AddIdentityServer()
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiScopes(Config.GetApiScopes())
                .AddDeveloperSigningCredential();

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

            app.UseRouting();

            app.UseIdentityServer();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }

    }
}

