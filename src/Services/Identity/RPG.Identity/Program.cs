using BuildingBlocks.Common;
using Serilog;

namespace RPG.Identity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddCommandLine(args)
                .AddEnvironmentVariables()
                .Build();

            //Log.Logger = CommonHostBuilder.DefaultLogger(configuration);

            var host = CommonHostBuilder.DefaultBuilder(args, typeof(Startup)).Build();
            //SeedDatabase(host);
            host.Run();
        }

        private static void SeedDatabase(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var config = services.GetRequiredService<IConfiguration>();
                    var connectionString = config.GetConnectionString("DefaultConnection");

                    SeedData.EnsureSeedData(connectionString);
                }
                catch (Exception ex)
                {
                    var logger =
                        services.GetRequiredService<ILogger<Program>>();
                    logger
                        .LogError("An error occurred while seeding the service database");
                    logger.LogError(ex.ToString());
                }
            }
        }
    }
}