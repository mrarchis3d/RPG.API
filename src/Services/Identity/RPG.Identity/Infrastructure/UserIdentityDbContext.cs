using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RPG.Identity.Domain.UserAggregate;

namespace RPG.Identity.Infrastructure
{
    public class UserIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public UserIdentityDbContext(DbContextOptions<UserIdentityDbContext> options): base(options)
        {
                
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
