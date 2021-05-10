// Test.WebApi/Test.WebApi/IdentityHostingStartup.cs
// James Allen
// 2021/05/10/2:09 PM

using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Test.WebApi.Areas.Identity;
using Test.WebApi.Areas.Identity.Data;
using Test.WebApi.Data;

[assembly: HostingStartup(typeof(IdentityHostingStartup))]
namespace Test.WebApi.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<TestWebApiContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("TestWebApiContextConnection")));

                services.AddDefaultIdentity<TestWebApiUser>(options => options.SignIn.RequireConfirmedAccount = false)
                    .AddEntityFrameworkStores<TestWebApiContext>();
            });
        }
    }
}