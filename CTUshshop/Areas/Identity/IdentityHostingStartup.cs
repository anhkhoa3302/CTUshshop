using System;
using CTUshshop.Areas.Identity.Data;
using CTUshshop.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(CTUshshop.Areas.Identity.IdentityHostingStartup))]
namespace CTUshshop.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<CTUshshopContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("CTUshshopContextConnection")));

                services.AddDefaultIdentity<ApplicationUser>(options =>
                    { 
                        options.SignIn.RequireConfirmedAccount = false;
                    }
                    ).AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<CTUshshopContext>();
            });
        }
    }
}