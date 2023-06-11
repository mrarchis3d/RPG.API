using Microsoft.EntityFrameworkCore;
using UserEntity = RPGAPI.Domain.UserAggregate.User;

namespace RPGAPI.Infrastructure
{
    public class ServiceDbContext : BaseContext<UserEntity>
    {
        public new DbSet<UserEntity> Users { get; set; }

        public ServiceDbContext(DbContextOptions<ServiceDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}