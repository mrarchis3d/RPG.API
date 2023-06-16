using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.EntityFramework.Storage;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using RPG.Identity.Domain.UserAggregate;
using RPG.Identity.Infrastructure;
using Serilog;
using System.Reflection;
using System.Security.Claims;

namespace RPG.Identity
{
    public class SeedData
    {
        public static void EnsureSeedData(string connectionString,bool isTest = false)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString,
              sqlServerOptionsAction: sqlOptions =>
              {
                  sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                  //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                  sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 15,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorNumbersToAdd: null
                    );
              }));

            services.AddIdentity<ApplicationUser, IdentityRole>()
              .AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders();

            services.AddOperationalDbContext(options =>
            {
                options.ConfigureDbContext = db =>
            db.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(typeof(SeedData).Assembly.FullName));
            });
            services.AddConfigurationDbContext(options =>
            {
                options.ConfigureDbContext = db =>
            db.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(typeof(SeedData).Assembly.FullName));
            });

            var serviceProvider = services.BuildServiceProvider();

            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                scope!.ServiceProvider!.GetService<PersistedGrantDbContext>()!.Database.Migrate();

                var context = scope.ServiceProvider.GetService<ConfigurationDbContext>();
                context!.Database!.Migrate();

                var ctx = scope.ServiceProvider.GetService<ApplicationDbContext>();
                ctx!.Database!.Migrate();

                EnsureSeedData(context);

                ApplyPostMigrations(ctx);

                if (isTest) EnsureUsers(scope);
            }
        }

        private static void EnsureUsers(IServiceScope scope)
        {
            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var bob = userMgr.FindByNameAsync("bob").Result;
            if (bob == null)
            {
                bob = new ApplicationUser
                {
                    UserName = "bob",
                    Name = "Bob Smith",
                    Email = "BobSmith@email.com",
                    PhoneNumber = "11231231234",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };
                var result = userMgr.CreateAsync(bob, "Pass123$").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddClaimsAsync(bob, new Claim[]
                {
                      new Claim(JwtClaimTypes.Name, "Bob Smith"),
                      new Claim(JwtClaimTypes.GivenName, "Bob"),
                      new Claim(JwtClaimTypes.FamilyName, "Smith"),
                      new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                      new Claim("location", "somewhere")
                }).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                Log.Debug("bob created");
            }
            else
            {
                Log.Debug("bob already exists");
            }

            var john = userMgr.FindByNameAsync("john").Result;
            if (john == null)
            {
                john = new ApplicationUser
                {
                    UserName = "john",
                    Name = "John Doe",
                    Email = "JohnDoe@email.com",
                    PhoneNumber = "11331231234",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };
                var result = userMgr.CreateAsync(john, "Pass123$").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddClaimsAsync(bob, new Claim[]
                {
                      new Claim(JwtClaimTypes.Name, "John Doe"),
                      new Claim(JwtClaimTypes.GivenName, "John"),
                      new Claim(JwtClaimTypes.FamilyName, "Doe"),
                      new Claim(JwtClaimTypes.WebSite, "http://john.com"),
                      new Claim("location", "somewhere")
                }).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                Log.Debug("john created");
            }
            else
            {
                Log.Debug("john already exists");
            }
        }

        private static void EnsureSeedData(ConfigurationDbContext context)
        {
            if (!context.Clients.Any())
            {
                Log.Debug("Clients being populated");
                foreach (var client in Config.Clients.ToList())
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
                foreach (var resource in Config.IdentityResources.ToList())
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
                foreach (var resource in Config.ApiScopes.ToList())
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
                foreach (var resource in Config.ApiResources.ToList())
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

        private static void ApplyPostMigrations(ApplicationDbContext context)
        {
            var users = context.Users.ToList();
            if (users.Any())
            {
                foreach (var user in users)
                {
                    if (!user.UserName.StartsWith(BuildingBlocks.Common.Constants.UserConstants.TEMPORAL_PREFIX))
                    {
                        user.LockoutEnd = DateTime.UtcNow.AddMinutes(-1);
                    }
                }

                context.Users.UpdateRange(users);

                context.SaveChanges();
            }
        }
    }
}
