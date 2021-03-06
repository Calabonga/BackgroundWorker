using Calabonga.BackgroundWorker.Api.Web.AppStart.Configures;
using Calabonga.BackgroundWorker.Api.Web.AppStart.ConfigureServices;
using Calabonga.BackgroundWorker.Api.Web.Infrastructure.DependencyInjection;
using Calabonga.BackgroundWorker.Api.Web.Infrastructure.Services;
using Calabonga.BackgroundWorker.Api.Web.Infrastructure.Working;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Calabonga.BackgroundWorker.Api.Web
{
    /// <summary>
    /// Entry point
    /// </summary>
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureServicesCommon.ConfigureServices(services, Configuration);
            ConfigureServicesAuthentication.ConfigureServices(services, Configuration);
            ConfigureServicesSwagger.ConfigureServices(services, Configuration);
            ConfigureServicesCors.ConfigureServices(services, Configuration);
            ConfigureServicesControllers.ConfigureServices(services);
            
            DependencyContainer.Common(services);

            // If redis enabled uncomment this settings
            //services.AddStackExchangeRedisCache(option =>
            //{
            //    option.Configuration = "localhost";
            //    option.InstanceName = "Works-Maintenance";
            //});
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="mapper"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AutoMapper.IConfigurationProvider mapper)
        {
            ConfigureCommon.Configure(app, env, mapper);
            ConfigureEndpoints.Configure(app);
            WorkerQueue.Instance.SetCache(app.ApplicationServices.GetRequiredService<IDistributedCacheService>());
        }
    }
}
