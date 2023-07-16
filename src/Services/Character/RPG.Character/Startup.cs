using FluentValidation;
using IdentityModel;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using RPG.BuildingBlocks.Common.Constants;
using RPG.BuildingBlocks.Common.Extensions;
using RPG.BuildingBlocks.Common.Middlewares;
using RPG.BuildingBlocks.Common.Providers.Identity;
using RPG.BuildingBlocks.Middlewares;
using RPG.BuildingBlocks.Utils;
using RPG.Character.Infrastructure;
using RPG.RPG.BuildingBlocks.Common.AuthorizationAttributes;
using System.Net.Http.Headers;

namespace RPG.Character
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
            string serviceId = Configuration["ServiceId"];
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
            string authorityUrl = Configuration["AUTHORITY_URL"];
            bool requireHttpsMetadata = bool.Parse(Configuration["AUTHORITY_REQUIRE_HTTPS_METADATA"]);
            Console.WriteLine($"Service Id {serviceId}");
            Console.WriteLine($"authority URL {authorityUrl}");
            Console.WriteLine($"requireHttpsMetadata {requireHttpsMetadata}");


            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddDbContext<ServiceDbContext>(options => options.UseSqlServer(connectionString));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
            services.AddValidatorsFromAssembly(typeof(Program).Assembly);
            services.AddAutoMapper(typeof(Program).Assembly);
            services.AddEndpointsApiExplorer();
            services.AddSingleton<IAuthorizationHandler, ValidApiTokenHandler>();
            services.AddControllers().AddNewtonsoftJson(x =>
            {
                x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                x.SerializerSettings.Formatting = Formatting.Indented;
            });//.AddDapr();

            //http://docs.identityserver.io/en/release/topics/apis.html
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
            services.AddHttpContextAccessor();

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
            services.AddTransient<IStringLocalizerFactory, ResourceManagerStringLocalizerFactory>();
            services.AddTransient(typeof(IStringLocalizer), typeof(StringLocalizer<CommonResources>));
            services.AddAuthorization();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }
            app.UseSwagger();

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

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseStaticFiles();
            app.UseMiddleware<AuthorizationPropagationMiddleware>();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

