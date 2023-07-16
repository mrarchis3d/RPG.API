using Microsoft.EntityFrameworkCore;
using RPG.BuildingBlocks.Common.BaseContext;
using RPG.Character.Domain.CharacterAggregate;
using CharacterEntity = RPG.Character.Domain.CharacterAggregate.Character;
using RPG.Character.Domain.UserAggregate;

namespace RPG.Character.Infrastructure
{
    public class ServiceDbContext : BaseContext<User>
    {
        public new DbSet<User> Users { get; set; }
        public new DbSet<CharacterEntity> Character { get; set; }
        public new DbSet<ClassType> ClassType { get; set; }
        public new DbSet<RaceType> RaceType { get; set; }
        public new DbSet<BaseAttributes> BaseAttributes { get; set; }


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