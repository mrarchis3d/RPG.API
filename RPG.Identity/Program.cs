using Microsoft.AspNetCore;
using RPG.Identity.Infrastructure;

namespace RPG.Identity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            SeedDatabase(host);
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
        private static void SeedDatabase(IWebHost host)
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