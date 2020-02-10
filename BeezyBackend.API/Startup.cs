using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BeezyBackend.API.Services;
using BeezyBackend.DataAccess.DBContext;
using BeezyBackend.DataAccess.Repositories;
using BeezyBackend.Domain.Repositories;
using BeezyBackend.Domain.Services;
using BeezyBackend.Domain.Services.QueryModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;

namespace BeezyBackend.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                .MinimumLevel.Override("System", LogEventLevel.Error)
                .WriteTo.File(outputTemplate: "{NewLine}{Timestamp:HH:mm:ss} [{Level:u3}] ({SourceContext}): {Message} {Exception}",
                    path: "..\\logs\\log.txt",
                    rollOnFileSizeLimit: true,
                    rollingInterval: RollingInterval.Day,
                    shared: true)
                .CreateLogger();

            services.RemoveAll<ILoggerProvider>();
            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(logger: logger, dispose: true));

            services.AddSingleton<IConfiguration>(Configuration);
            services.AddDbContext<BeezyCinemaContext>(options => options.UseSqlServer(Configuration.GetConnectionString("BeezyBackend"), m => m.MigrationsAssembly("BeezyBackend.DataAccess")));

            services.AddMvc()
                 .AddMvcOptions(options => options.EnableEndpointRouting = false)
                 .AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BeezyBackend API", Version = "v1" });
                c.DescribeAllEnumsAsStrings();
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            //IoC
            services.AddScoped<IMoviesRepository, MovieRepository>();
            services.AddSingleton<IUtc, Utc>();
            services.AddSingleton<IWeekDates, WeekDates>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BeezyBackend API v1"));
            app.UseMvc();
        }
    }
}
