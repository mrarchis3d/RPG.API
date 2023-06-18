using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RPG.Identity.Domain.UserAggregate;

namespace RPG.Identity.Infrastructure
{
    public class UserIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IConfiguration _configuration;
        public UserIdentityDbContext(DbContextOptions<UserIdentityDbContext> options, IConfiguration configuration): base(options)
        {
                _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //string serviceId = _configuration["ServiceId"];
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            optionsBuilder
                    .UseSqlServer(
                        connectionString,
                        o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
