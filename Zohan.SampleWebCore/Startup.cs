using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zohan.SampleWebCore.Models;

namespace Zohan.SampleWebCore
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
            // Add framework services
            services.AddMvc();

            // Configure Application Insights
            var instrumentationKey = Configuration.GetSection("ApplicationInsights")["InstrumentationKey"];
            services.AddApplicationInsightsTelemetry(instrumentationKey);

            // Add application services
            services.AddTransient<ITelemetryTracker>(tracker => new TelemetryTracker(instrumentationKey));
            services.Configure<DocumentDbSettings>(Configuration.GetSection("DocumentDBSettings"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
