using Microsoft.EntityFrameworkCore;
using RPG.Identity.Domain.UserAggregate;

namespace RPG.Identity.Infrastructure
{
    public abstract class BaseContext<TUser> : DbContext where TUser : BaseUser
    {
        public DbSet<TUser> Users { get; set; }

        protected BaseContext(DbContextOptions options)
            : base(options)
        {
        }
    }
}
