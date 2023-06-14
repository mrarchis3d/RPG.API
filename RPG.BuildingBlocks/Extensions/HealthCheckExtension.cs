using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace RPG.BuildingBlocks.Common.Extensions
{
    public static class HealthCheckExtension
    {
        public enum ServiceType
        {
            SQL,
            MONGO,
            AGGREGATOR,
            NO_DB
        }
        public static IServiceCollection AddCustomHealthCheck(this IServiceCollection services, IConfiguration configuration, ServiceType serviceType)
        {
            var hcBuilder = services.AddHealthChecks();
            string secretStoreSectionName = configuration["ServiceId"];

            hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());
            hcBuilder.AddProcessAllocatedMemoryHealthCheck(512);

            switch (serviceType)
            {
                case ServiceType.SQL:
                    AddCustomHealthCheckSql(hcBuilder, configuration, secretStoreSectionName);
                    break;
                case ServiceType.MONGO:
                    AddCustomHealthCheckMongo(hcBuilder, configuration, secretStoreSectionName);
                    break;
                case ServiceType.AGGREGATOR:
                    break;
                case ServiceType.NO_DB:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(serviceType), serviceType, null);
            }

            return services;
        }
        public static IHealthChecksBuilder AddCustomHealthCheckSql(this IHealthChecksBuilder hcBuilder, IConfiguration configuration, string serviceName)
        {
            string dbName = $"{serviceName}_db";
            string dbConnectionString = configuration[dbName];

            hcBuilder
                .AddSqlServer(
                    dbConnectionString,
                    name: $"{dbName}-check",
                    tags: new[] { dbName });

            return hcBuilder;
        }

        public static IHealthChecksBuilder AddCustomHealthCheckMongo(this IHealthChecksBuilder hcBuilder, IConfiguration configuration, string serviceName)
        {
            string dbName = $"{serviceName}_db";
            string dbConnectionString = configuration[dbName];

            hcBuilder
                .AddMongoDb(
                    dbConnectionString,
                    name: $"{dbName}-check",
                    tags: new[] { dbName });

            return hcBuilder;
        }
    }
}