using Identity.API.Data;
using Identity.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var seed = args.Contains("/seed");
if (seed)
{
    args = args.Except(new[] { "/seed" }).ToArray();
}

var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly.GetName().Name;
var Connectstring = builder.Configuration.GetConnectionString("DefaultConnection");

if (seed)
{
    SeedData.EnsureSeedData(Connectstring);
}


builder.Services.AddDbContext<AspNetIdentityDbContext>(option =>
                {
                    option.UseSqlServer(Connectstring, db => db.MigrationsAssembly(assembly));
                });

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AspNetIdentityDbContext>();

builder.Services.AddIdentityServer()
                .AddAspNetIdentity<IdentityUser>()
                .AddConfigurationStore(option =>
                {
                    option.ConfigureDbContext = db =>
                    {
                        db.UseSqlServer(Connectstring, option => option.MigrationsAssembly(assembly));
                    };
                })
                .AddOperationalStore(option =>
                {
                    option.ConfigureDbContext = db =>
                    {
                        db.UseSqlServer(Connectstring, option => option.MigrationsAssembly(assembly));
                    };
                })
                .AddDeveloperSigningCredential();

builder.Services.AddControllersWithViews();
builder.Services.AddControllers();

var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});
app.Run();
