using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LandonApi.Filters;
using LandonApi.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LandonApi
{
    public class Startup
    {
        private readonly int? _httpsPort;

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;

            // Get the HTTPS port (only in development)
            if(Environment.IsDevelopment())
            {
                var lauchJsonConfig = new ConfigurationBuilder()
                    .SetBasePath(Environment.ContentRootPath)
                    .AddJsonFile("Properties/launchSettings.json")
                    .Build();

                _httpsPort = lauchJsonConfig.GetValue<int>("iisSettings:iisExpress:sslPort");
            }
        }

        public IConfiguration Configuration { get; }

        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(opt =>
            {
                // Use JsonExceptionErro
                opt.Filters.Add(typeof(JsonExceptionFilter));


                // Require HTTPS for add controllers
                opt.SslPort = _httpsPort;
                opt.Filters.Add(typeof(RequireHttpsAttribute));

                var jsonFormatter = opt.OutputFormatters.OfType<JsonOutputFormatter>().Single();
                opt.OutputFormatters.Remove(jsonFormatter);

                opt.OutputFormatters.Add(new IonOutputFormatter(jsonFormatter));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddRouting(opt => opt.LowercaseUrls = true);

            services.AddApiVersioning(opt =>
            {
                // Use media type versioning
                opt.ApiVersionReader = new MediaTypeApiVersionReader();
                // Assume default version if there's no version requested or specified
                opt.AssumeDefaultVersionWhenUnspecified = true;
                // Version to be support in headers in the response
                opt.ReportApiVersions = true;
                // Default API version 
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                // API version selector to a new instance of current implementation API version selector
                // The current implementation version selector will automatically use the highest or 
                // newest version of a route if no version is requested by the client 
                opt.ApiVersionSelector = new CurrentImplementationApiVersionSelector(opt);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHsts(opt => {
                // How long browser will remember the HSTS setting
                opt.MaxAge(days: 180);
                // Include subdomains flag => HSTS applies not just to the root domain 
                // of this API, but any potential sub-domains
                opt.IncludeSubdomains();
                // browser allowed to assume the site use HSTS if the site submitted
                // to a common list of HSTS enabled websites
                opt.Preload();
            });
            
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
