﻿using Calabonga.BackgroundWorker.Api.Data;
using Calabonga.BackgroundWorker.Api.Web.Infrastructure.Auth;
using Calabonga.BackgroundWorker.Api.Web.Infrastructure.Services;

using IdentityServer4.Services;

using Microsoft.Extensions.DependencyInjection;

namespace Calabonga.BackgroundWorker.Api.Web.Infrastructure.DependencyInjection
{
    /// <summary>
    /// Registrations for both points: API and Scheduler
    /// </summary>
    public partial class DependencyContainer
    {
        /// <summary>
        /// Register 
        /// </summary>
        /// <param name="services"></param>
        public static void Common(IServiceCollection services)
        {
            services.AddTransient<ApplicationUserStore>();
            services.AddTransient<IApplicationDbContext, ApplicationDbContext>();
            services.AddScoped<ApplicationClaimsPrincipalFactory>();

            // services
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IProfileService, IdentityProfileService>();
            services.AddTransient<ICacheService, CacheService>();
            services.AddTransient<ICorsPolicyService, IdentityServerCorsPolicy>();
        }
    }
}
