using Microsoft.EntityFrameworkCore;

namespace RPG.BuildingBlocks.Common.BaseContext
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
