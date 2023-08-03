using System;
using Abp.AspNetCore;
using Abp.Castle.Logging.Log4Net;
using Abp.EntityFrameworkCore;
using Atlas.Legal.EntityFrameworkCore;
using Castle.Facilities.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Text;
using Atlas.Legal.Configuration;
using Atlas.Legal.Web.Middleware;
using Abp;
using Quartz;
using Abp.Quartz;
using Microsoft.IdentityModel.Logging;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using MvcMovie.Controllers;
using Microsoft.AspNetCore.Http;

namespace Atlas.Legal.Web.Startup
{
    public class Startup
    {
        private readonly IConfigurationRoot _config;
        readonly string specificOrigins = "specificOrigins";

        public Startup(IHostEnvironment env)
        {
            _config = AppConfigurations.Get(env.ContentRootPath, env.EnvironmentName);
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //string[] corsOrigins = Configuration.AppConfigurations.Get("AllowedHosts").Get<string[]>();
            //var assembly = typeof(HelloWorldController).Assembly;

            // ApplicationPart
            /*services.AddControllers()
                    .AddApplicationPart(assembly);*/

            /* {
                 var assembly = typeof(HelloWorldController).Assembly;
                 var externalController = new AssemblyPart(assembly);

                 // ApplicationPartManager
                 services.AddControllers()
                         .ConfigureApplicationPartManager(apm =>
                         {
                             apm.ApplicationParts.Add(externalController);
                         });
             }*/
            services.AddTransient<HelloWorldController>();
            services.AddCors(options =>
            {
                options.AddPolicy(specificOrigins,
                                    builder =>
                                    {
                                        builder
                                        //.WithOrigins(corsOrigins)
                                        .AllowAnyOrigin()
                                        .AllowAnyHeader()
                                        .AllowAnyMethod();
                                        //.AllowCredentials();
                                        
                                    });
            });
           

            //Configure DbContext
            services.AddAbpDbContext<LegalDbContext>(options =>
            {
                DbContextOptionsConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
            });

            services.AddHttpContextAccessor();

            services.AddControllersWithViews(options =>
            {
                //options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            }).AddNewtonsoftJson();
            services.AddTransient<IQuartzScheduleJobManager, QuartzScheduleJobManager>();

            //Configure Abp and Dependency Injection
            return services.AddAbp<LegalWebModule>(options =>
            {
                //Configure Log4Net logging
                options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseAbpLog4Net().WithConfig("log4net.config")
                );
            });          
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseCors(specificOrigins);
            app.UseAbp(); //Initializes ABP framework.

            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                IdentityModelEventSource.ShowPII = true;
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            //app.UseStaticFiles();
            
            app.UseRouting();            
            app.UseValidaJwt();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", (context) => context.Response.WriteAsync("Response:{Success:200 secrets}"));
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
