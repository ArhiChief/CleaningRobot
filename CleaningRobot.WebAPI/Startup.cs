using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CleaningRobot.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using CleaningRobot.WebAPI.Infrastructure;

namespace CleaningRobot.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvcCore() // we no need to full support of MVC like Razor and other. Just WebAPI, just hardcore
                .AddJsonFormatters(setup =>
                {
                    setup.Converters = new List<JsonConverter>
                    {
                        new CommandConverter(),
                        new FacingDirectionConverter(),
                        new MapCellConverter()
                    };
                    setup.NullValueHandling = NullValueHandling.Ignore; // we don't want to send null values back to user 
                });

            // add our stuff to DI container
            services.AddSingleton<IRobotManager>(new RobotManager()); // no need to create instance for each WebAPI call
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(config =>
                {
                    config.Run(
                        async context =>
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            context.Response.ContentType = "application/json";
                            var ex = context.Features.Get<IExceptionHandlerFeature>();
                            if (ex != null)
                            {
                                var err = $"{{\"Error\":\"{ex.Error.Message}\"}}";
                                await context.Response.WriteAsync(err).ConfigureAwait(false);
                            }
                        }
                    );
                });
            }

            app.UseMvc();
        }
    }
}
