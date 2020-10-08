using Calabonga.BackgroundWorker.Api.Data;
using Calabonga.BackgroundWorker.Api.Web.Infrastructure.Auth;
using Calabonga.BackgroundWorker.Api.Web.Infrastructure.Services;
using Calabonga.BackgroundWorker.Api.Web.Infrastructure.Working;
using Calabonga.BackgroundWorker.Api.Web.Infrastructure.Working.Jobs;
using IdentityServer4.Services;

using Microsoft.Extensions.DependencyInjection;

namespace Calabonga.BackgroundWorker.Api.Web.Infrastructure.DependencyInjection
{
    /// <summary>
    /// Registrations for both points: API and Scheduler
    /// </summary>
    public class DependencyContainer
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
            services.AddTransient<IDistributedCacheService, DistributedCacheService>();
            services.AddTransient<ICorsPolicyService, IdentityServerCorsPolicy>();

            services.AddTransient<IWorkService, Worker>();
            services.AddTransient<IWorkerProcessor, WorkerProcessor>();
            services.AddTransient<IWorker, Worker>();
            
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
            services.AddTransient<BackgroundJob>();
        }
    }
}
