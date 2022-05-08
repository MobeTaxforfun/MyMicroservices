using Identity.API.Configuration;
using Identity.API.Models;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.EntityFramework.Storage;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Identity.API.Data
{
    public class SeedData
    {
        public static void EnsureSeedData(string connectstring)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<AspNetIdentityDbContext>(
                option => option.UseSqlServer(connectstring)
                );

            services
                .AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AspNetIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddOperationalDbContext(
                option =>
                {
                    option.ConfigureDbContext = db =>
                        db.UseSqlServer(connectstring, sql => sql.MigrationsAssembly(typeof(SeedData).Assembly.FullName));
                });

            services.AddConfigurationDbContext(
                option =>
                {
                    option.ConfigureDbContext = db =>
                        db.UseSqlServer(connectstring, sql => sql.MigrationsAssembly(typeof(SeedData).Assembly.FullName));
                });

            var serviceProvider = services.BuildServiceProvider();

            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            scope.ServiceProvider.GetService<PersistedGrantDbContext>().Database.Migrate();

            var context = scope.ServiceProvider.GetService<ConfigurationDbContext>();
            context.Database.Migrate();

            EnsureSeedData(context);

            var ctx = scope.ServiceProvider.GetService<AspNetIdentityDbContext>();
            ctx.Database.Migrate();
            EnsureUsers(scope);

        }

        private static void EnsureUsers(IServiceScope scope)
        {
            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            var MobeWu = userMgr.FindByNameAsync("MobeWu").Result;
            if (MobeWu == null)
            {
                MobeWu = new IdentityUser
                {
                    UserName = "MobeWu",
                    Email = "MobeWu@email.com",
                    EmailConfirmed = true
                };
                var result = userMgr.CreateAsync(MobeWu, "$Mobewu7414").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result =
                    userMgr.AddClaimsAsync(
                        MobeWu,
                        new Claim[]
                        {
                            new Claim(JwtClaimTypes.Name, "MobeWu"),
                            new Claim(JwtClaimTypes.GivenName, "Mobe"),
                            new Claim(JwtClaimTypes.FamilyName, "Wu"),
                            new Claim(JwtClaimTypes.WebSite, "https://xxx.com"),
                            new Claim("location", "TW-zh")
                        }
                    ).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
            }
        }

        private static void EnsureSeedData(ConfigurationDbContext context)
        {
            if (!context.Clients.Any())
            {
                foreach (var client in Config.Clients.ToList())
                {
                    context.Clients.Add(client.ToEntity());
                }

                context.SaveChanges();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in Config.IdentityResources.ToList())
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }

            if (!context.ApiScopes.Any())
            {
                foreach (var resource in Config.ApiScopes.ToList())
                {
                    context.ApiScopes.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }

            if (!context.ApiResources.Any())
            {
                foreach (var resource in Config.ApiResources.ToList())
                {
                    context.ApiResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }
        }
    }
}
