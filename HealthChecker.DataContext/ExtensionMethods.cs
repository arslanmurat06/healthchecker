
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace HealthChecker.DataContext
{
    public static class ExtensionMethods
    {
        public static void UseApplicationDbContext(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("HealthCheckerWeb"));
            });
        }

        public static void UseDefaultIdentity(this IServiceCollection services)
        {
            services.AddDefaultIdentity<IdentityUser>(options => {
                options.SignIn.RequireConfirmedAccount = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>();
        }
    }
}
