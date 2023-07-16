using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using RPG.Character.Domain.CharacterAggregate;
using CharacterEntity = RPG.Character.Domain.CharacterAggregate.Character;

namespace RPG.Character.Infrastructure.EntityConfiguration
{
    public class ClassTypeConfiguration : IEntityTypeConfiguration<ClassType>
    {
        public void Configure(EntityTypeBuilder<ClassType> builder)
        {
            builder.ToTable("ClassTypes");

            builder.HasKey(ct => ct.Id);

            builder.Property(ct => ct.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasOne(ct => ct.Character)
                .WithOne(c => c.ClassType)
                .HasForeignKey<CharacterEntity>(c => c.ClassTypeId);
        }
    }
}
