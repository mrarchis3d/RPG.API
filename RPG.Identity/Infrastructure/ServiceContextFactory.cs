using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RPG.Identity.Infrastructure
{
    public class ServiceContextFactory : IDesignTimeDbContextFactory<ServiceDbContext>
    {
        public ServiceDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ServiceDbContext>();

            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=RPG.Identity;User Id=sa;password=Test123!;TrustServerCertificate=true");

            return new ServiceDbContext(optionsBuilder.Options);
        }
    }
}
       