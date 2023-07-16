using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RPG.Character.Domain.UserAggregate;

namespace RPG.Character.Infrastructure.EntityConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.HasKey(e => e.UserId);

            builder.Property(c => c.FullName)
                .IsRequired(true)
                .HasMaxLength(150);

            builder.Property(c => c.UserName)
                .IsRequired(true)
                .HasMaxLength(256);
        }
    }
}
