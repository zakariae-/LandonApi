using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LandonApi.Filters;
using LandonApi.Infrastructure;
using LandonApi.Models;
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
using Microsoft.EntityFrameworkCore;
using LandonApi.Services;
using AutoMapper;

namespace LandonApi
{
    public class Startup
    {
        private readonly int? _httpsPort;

        public Startup(IHostingEnvironment environment)
        {
            //Configuration = configuration;
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);


            Environment = environment;

            // Get the HTTPS port (only in development)
            if(Environment.IsDevelopment())
            {
                builder.AddJsonFile($"appsettings.{Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);
                var lauchJsonConfig = new ConfigurationBuilder()
                    .SetBasePath(Environment.ContentRootPath)
                    .AddJsonFile("Properties/launchSettings.json")
                    .Build();

                _httpsPort = lauchJsonConfig.GetValue<int>("iisSettings:iisExpress:sslPort");
            }

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Use an in-memory database for quick dev and test
            // TODO : Swap out with a real database
            services.AddDbContext<HotelApiContext>(opt => opt.UseInMemoryDatabase("HotelDatabase"));

            services.AddAutoMapper();

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


            services.Configure<HotelInfo>(Configuration.GetSection("Info"));

            services.AddScoped<IRoomService, DefaultRoomService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // Add some test data in development
                var context = app.ApplicationServices.GetRequiredService<HotelApiContext>();
                AddTestData(context);
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

        private static void AddTestData(HotelApiContext context)
        {
            context.Rooms.Add(new RoomEntity
            {
                Id = Guid.Parse("d88eb383-f15f-4b3a-9770-744516ffa85a"),
                Name = "Oxford",
                Rate = 123445
            });
            context.Rooms.Add(new RoomEntity
            {
                Id = Guid.Parse("ef57eea0-2cc6-4f27-9754-fef9ace14977"),
                Name = "Oxford",
                Rate = 12344
            });

            context.SaveChanges();
        }
    }
}
