using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RPG.Character.Domain.CharacterAggregate;
using CharacterEntity = RPG.Character.Domain.CharacterAggregate.Character;


namespace RPG.Character.Infrastructure.EntityConfiguration
{
    public class RaceTypeConfiguration : IEntityTypeConfiguration<RaceType>
    {
        public void Configure(EntityTypeBuilder<RaceType> builder)
        {
            builder.ToTable("RaceTypes");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(r => r.Description)
                .HasMaxLength(1250);

            builder.HasOne(r => r.Character)
                .WithOne(c => c.RaceType)
                .HasForeignKey<CharacterEntity>(c => c.RaceTypeId);
        }
    }
}
