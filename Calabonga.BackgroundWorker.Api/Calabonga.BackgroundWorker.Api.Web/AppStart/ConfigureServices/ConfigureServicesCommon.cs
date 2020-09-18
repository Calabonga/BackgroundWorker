using System;

using AutoMapper;

using Calabonga.BackgroundWorker.Api.Data;
using Calabonga.BackgroundWorker.Api.Web.Extensions;
using Calabonga.BackgroundWorker.Api.Web.Infrastructure.Settings;
using Calabonga.UnitOfWork;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Calabonga.BackgroundWorker.Api.Web.AppStart.ConfigureServices
{
    /// <summary>
    /// ASP.NET Core services registration and configurations
    /// </summary>
    public static class ConfigureServicesCommon
    {
        /// <summary>
        /// ConfigureServices Services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddIdentity<ApplicationUser, ApplicationRole>()
                .AddUserStore<ApplicationUserStore>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddDbContextPool<ApplicationDbContext>(config =>
            {
                config.UseSqlServer(configuration.GetConnectionString(nameof(ApplicationDbContext)));
            });

            services.AddAutoMapper(typeof(Startup));

            services.AddUnitOfWork<ApplicationDbContext>();

            services.AddMemoryCache();

            services.AddRouting(options => options.LowercaseUrls = true);

            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters = null;
                options.User.RequireUniqueEmail = true;
            });

            services.AddOptions();
            services.Configure<CurrentAppSettings>(configuration.GetSection(nameof(CurrentAppSettings)));
            services.Configure<MvcOptions>(options => options.UseRouteSlugify());
            services.AddLocalization();
            services.AddHttpContextAccessor();
            services.AddResponseCaching();
            services.AddAuthorization();
        }
    }
}