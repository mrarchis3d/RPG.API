using BuildingBlocks.Common;
using Dapr.Client;
using Dapr.Extensions.Configuration;
using RPG.BuildingBlocks.Common.Infrastructure;
using RPG.Character.Infrastructure;
using Serilog;

namespace RPG.Character
{
    public class Program
    {

        public static void Main(string[] args)
        {
             var host = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).Build();
            SeedDatabase(host);
            host.Run();
        }

        private static void SeedDatabase(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<ServiceDbContext>();
                    new ServiceSeeding().SeedAsync(context/*, services*/).Wait();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError("An error occurred while seeding the service database");
                    logger.LogError(ex.ToString());
                }
            }
        }
    }
}