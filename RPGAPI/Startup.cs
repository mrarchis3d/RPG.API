using FluentValidation;
using MediatR;
using Microsoft.OpenApi.Models;
using RPGAPI.Domain.UserAggregate;
using RPGAPI.Infrastructure;
using RPGAPI.Services;
using RPGAPI.Services.Interfaces;
using System.Reflection;

namespace RPGAPI
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


            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(typeof(Startup));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RPG API", Version = "v1" });
            });
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<BaseContext<User>, ServiceDbContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {

                // Habilita la generación del documento JSON de Swagger
                app.UseSwagger();

                // Especifica la ruta para acceder al documento JSON de Swagger
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "RPG.API");
                });
                app.UseDeveloperExceptionPage();

            }

        }

    }
}

