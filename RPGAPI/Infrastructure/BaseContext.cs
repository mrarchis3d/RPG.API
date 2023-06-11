using Microsoft.EntityFrameworkCore;
using RPGAPI.Domain.UserAggregate;

namespace RPGAPI.Infrastructure
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
