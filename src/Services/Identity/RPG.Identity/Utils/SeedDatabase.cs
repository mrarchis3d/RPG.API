using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;
using RPG.Identity.Infrastructure;
using Serilog;

namespace RPG.Identity.Utils
{
    public static class SeedDatabase {
        public static void IdentityDatabase(this IApplicationBuilder app)
        {
            using (var serviceScope =
                    app.ApplicationServices!
                        .GetService<IServiceScopeFactory>()!.CreateScope())
            {
                var identity = serviceScope.ServiceProvider.GetRequiredService<UserIdentityDbContext>();

                identity.Database.Migrate();

                var persistence = serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();
                  
                persistence.Database.Migrate();

                var context =serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

                EnsureSeedData(context);

            }
        }

        private static void EnsureSeedData(ConfigurationDbContext context)
        {
            if (!context.Clients.Any())
            {
                Log.Debug("Clients being populated");
                foreach (var client in ClientConfig.GetClients())
                {
                    context.Clients.Add(client.ToEntity());
                }

                context.SaveChanges();
            }
            else
            {
                Log.Debug("Clients already populated");
            }

            if (!context.IdentityResources.Any())
            {
                Log.Debug("IdentityResources being populated");
                foreach (var resource in Config.GetIdentityResources())
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }
            else
            {
                Log.Debug("IdentityResources already populated");
            }

            if (!context.ApiScopes.Any())
            {
                Log.Debug("ApiScopes being populated");
                foreach (var resource in Config.GetApiScopes())
                {
                    context.ApiScopes.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }
            else
            {
                Log.Debug("ApiScopes already populated");
            }

            if (!context.ApiResources.Any())
            {
                Log.Debug("ApiResources being populated");
                foreach (var resource in Config.GetApiResources())
                {
                    context.ApiResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }
            else
            {
                Log.Debug("ApiScopes already populated");
            }
        }

    }
    
}
