using Calabonga.AspNetCore.Controllers.Extensions;
using Calabonga.BackgroundWorker.Api.Web.Infrastructure.HostedServices;
using Microsoft.Extensions.DependencyInjection;

namespace Calabonga.BackgroundWorker.Api.Web.AppStart.ConfigureServices
{
    /// <summary>
    /// Configure controllers
    /// </summary>
    public static class ConfigureServicesControllers
    {
        /// <summary>
        /// Configure services
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddHostedService<EveryMinuteHostedService>();

            services.AddCommandAndQueries(typeof(Startup).Assembly);
        }
    }
}
