using System;
using System.Collections.Generic;
using System.IO;
using Dapr.Client;
using Dapr.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;
using Serilog.Filters;

namespace RPG.BuildingBlocks.Common.Infrastructure
{
    public static class OurglassHostBuilder
    {
        public static IHostBuilder DefaultBuilder(string[] args, Type startup)
        {
            return Host
                .CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(config =>
                    config
                        .UseStartup(startup)
                )
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    if (builderContext.HostingEnvironment.IsDevelopment())
                    {
                        var daprClient = new DaprClientBuilder().Build();
                        config.AddDaprSecretStore("ourglass-secrets", daprClient);
                    }
                    else if (builderContext.HostingEnvironment.IsProduction()
                             || builderContext.HostingEnvironment.IsStaging()) //Secrets on Kubernetes
                    {
                        var daprClient = new DaprClientBuilder().Build();
                        var secretDescriptors =
                            new List<DaprSecretDescriptor>
                            {
                                new DaprSecretDescriptor("ourglass-secrets")
                            };
                        config
                            .AddDaprSecretStore(Environment.GetEnvironmentVariable("SECRET_STORE"),
                                secretDescriptors,
                                daprClient);
                    }
                })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseSerilog();
        }

        public static Logger DefaultLogger(IConfiguration configuration)
        {

            var loggerBuilder = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration);

            loggerBuilder = loggerBuilder.WriteTo.Console()
                .Enrich.FromLogContext()
                .Filter.ByExcluding(Matching.WithProperty<string>("RequestPath", v => "/hc".Equals(v, StringComparison.OrdinalIgnoreCase)))
                .Filter.ByExcluding(Matching.WithProperty<string>("RequestPath", v => "/liveness".Equals(v, StringComparison.OrdinalIgnoreCase)));

            if (!string.IsNullOrEmpty(configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT", string.Empty)) && configuration["ASPNETCORE_ENVIRONMENT"].ToLower() == "development")
            {
                var seqUri = configuration.GetServiceUri("seq").ToString();
                loggerBuilder = loggerBuilder.WriteTo.Seq(seqUri);
            }

            return loggerBuilder.CreateLogger();
        }
    }
}
