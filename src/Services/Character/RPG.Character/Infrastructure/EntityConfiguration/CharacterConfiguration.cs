using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RPG.Character.Domain.CharacterAggregate;

namespace Ourglass.Spaces.Infrastructure.EntityConfiguration
{
    public class CharacterConfiguration : IEntityTypeConfiguration<Character>
    {
        public void Configure(EntityTypeBuilder<Character> builder)
        {
            builder.ToTable("Characters");

            builder.HasKey(c => c.UserId);

            builder.Property(c => c.UserId)
                .IsRequired();

            builder.Property(c => c.NickName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(c => c.Description)
                .HasMaxLength(200);

            builder.Property(c => c.Level)
                .IsRequired();

            builder.Property(c => c.Experience)
                .IsRequired();

            builder.Property(c => c.ProfessionLevel)
                .IsRequired();

            builder.Property(c => c.ProfessionExperience)
                .IsRequired();

            builder.Property(c => c.ClassTypeId)
                .IsRequired();

            builder.Property(c => c.RaceTypeId)
                .IsRequired();

            builder.HasOne(c => c.User)
                .WithOne(u => u.Character)
                .HasForeignKey<Character>(c => c.UserId)
                .IsRequired();

            builder.HasOne(c => c.ClassType)
                .WithMany()
                .HasForeignKey(c => c.ClassTypeId)
                .IsRequired();

            builder.HasOne(c => c.RaceType)
                .WithMany()
                .HasForeignKey(c => c.RaceTypeId)
                .IsRequired();
        }
    }
}
