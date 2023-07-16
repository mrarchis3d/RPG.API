using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RPG.Character.Domain.CharacterAggregate;

namespace RPG.Character.Infrastructure.EntityConfiguration
{
    public class BaseAttributesConfiguration : IEntityTypeConfiguration<BaseAttributes>
    {
        public void Configure(EntityTypeBuilder<BaseAttributes> builder)
        {
            builder.ToTable("BaseAttributes");

            builder.HasKey(ba => ba.Id);

            builder.Property(ba => ba.ClassTypeId)
                .IsRequired(false);

            builder.Property(ba => ba.RaceTypeId)
                .IsRequired(false);

            builder.Property(ba => ba.PhisycalDamageLow)
                .IsRequired();

            builder.Property(ba => ba.PhisycalDamageHigh)
                .IsRequired();

            builder.Property(ba => ba.CriticRatePercentage)
                .IsRequired();

            builder.Property(ba => ba.BaseHealth)
                .IsRequired();

            builder.Property(ba => ba.MagicalDamageLow)
                .IsRequired();

            builder.Property(ba => ba.MagicalDamageHigh)
                .IsRequired();

            builder.Property(ba => ba.Concentration)
                .IsRequired();

            builder.Property(ba => ba.DodgePercentage)
                .IsRequired();

            builder.Property(ba => ba.Armor)
                .IsRequired();

            builder.Property(ba => ba.MagicalArmor)
                .IsRequired();

            builder.HasOne(ba => ba.RaceType)
                .WithMany()
                .HasForeignKey(ba => ba.RaceTypeId)
                .IsRequired(false);

            builder.HasOne(ba => ba.ClassType)
                .WithMany()
                .HasForeignKey(ba => ba.ClassTypeId)
                .IsRequired(false);
        }
    }
}
