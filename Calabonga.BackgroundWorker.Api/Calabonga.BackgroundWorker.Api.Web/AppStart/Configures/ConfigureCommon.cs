﻿using Calabonga.BackgroundWorker.Api.Core;
using Calabonga.BackgroundWorker.Api.Web.AppStart.ConfigureServices;
using Calabonga.BackgroundWorker.Api.Web.Middlewares;
using Calabonga.Microservices.Web.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Calabonga.BackgroundWorker.Api.Web.AppStart.Configures
{
    /// <summary>
    /// Pipeline configuration
    /// </summary>
    public static class ConfigureCommon
    {
        /// <summary>
        /// Configure pipeline
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="mapper"></param>
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env, AutoMapper.IConfigurationProvider mapper)
        {
            if (env.IsDevelopment())
            {
                mapper.AssertConfigurationIsValid();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                mapper.CompileMappings();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=600");
                }
            });

            app.UseResponseCaching();

            app.UseETagger();

            app.Map($"{AppData.AuthUrl}", authServer => { authServer.UseIdentityServer(); });

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            app.UseSwaggerUI(ConfigureServicesSwagger.SwaggerSettings);

            app.UseSwagger();

            // Singleton setup for User Identity
            UserIdentity.Instance.Configure(app.ApplicationServices.GetService<IHttpContextAccessor>());
        }
    }
}
