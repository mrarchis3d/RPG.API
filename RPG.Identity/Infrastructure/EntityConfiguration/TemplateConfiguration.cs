using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RPG.Identity.Domain.SpaceAggregate;

namespace RPG.Identity.Infrastructure.EntityConfiguration
{
    public class TemplateConfiguration : IEntityTypeConfiguration<Template>
    {
        public void Configure(EntityTypeBuilder<Template> builder)
        {
            builder.ToTable("Template");

        }
    }
}
