using Microsoft.EntityFrameworkCore;

namespace RPGAPI.Infrastructure
{
    public class ServiceSeeding
    {
        public async Task SeedAsync(ServiceDbContext context)
        {
            await context.Database.MigrateAsync();

            //if (!await context.Templates.AnyAsync())
            //{
            //    var templates = new List<Template>();
            //    var id = Guid.NewGuid();
            //    templates.Add(new Template
            //    {
            //        Name = "Family",
            //        Id = id,
            //        CreatorRoleName = "Creator",
            //        NewUserDefaultRoleName = "Guest",
            //        TemplateRoles = new List<TemplateRole> {
            //            new TemplateRole {
            //                Name = "Creator",
            //                Id = Guid.NewGuid(),
            //                IsAdmin = true,
            //                IsModerator = false,
            //                CanPostAsSpace = true,
            //                TemplateId = id
            //            },
            //            new TemplateRole {
            //                Name = "Guest",
            //                Id = Guid.NewGuid(),
            //                IsAdmin = false,
            //                IsModerator = false,
            //                CanPostAsSpace = false,
            //                TemplateId = id,
            //            }
            //        }
            //    });

            //    id = Guid.NewGuid();
            //    templates.Add(new Template
            //    {
            //        Name = "Band",
            //        Id = Guid.NewGuid(),
            //        CreatorRoleName = "Creator",
            //        NewUserDefaultRoleName = "Guest",
            //        TemplateRoles = new List<TemplateRole> {
            //            new TemplateRole {
            //                Name = "Creator",
            //                Id = Guid.NewGuid(),
            //                IsAdmin = true,
            //                IsModerator = false,
            //                CanPostAsSpace = true,
            //                TemplateId = id,
            //            },
            //            new TemplateRole {
            //                Name = "Guest",
            //                Id = Guid.NewGuid(),
            //                IsAdmin = false,
            //                IsModerator = false,
            //                CanPostAsSpace = false,
            //                TemplateId = id,
            //            }
            //        }
            //    });


            //    context.Templates.AddRange(templates);
            //    context.SaveChanges();
            //}
        }
    }
}