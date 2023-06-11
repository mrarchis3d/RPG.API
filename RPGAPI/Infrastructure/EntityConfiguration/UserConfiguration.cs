using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RPGAPI.Domain.UserAggregate;

namespace RPGAPI.Infrastructure.EntityConfiguration
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

            builder.Property(c => c.PasswordHash)
                .IsRequired(true);

            builder.Property(c => c.Salt)
                .IsRequired(true);
        }
    }
}
