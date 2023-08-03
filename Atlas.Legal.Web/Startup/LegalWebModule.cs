using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.AspNetCore.Mvc.ExceptionHandling;
using Abp.Modules;
using Abp.Quartz;
using Abp.Reflection.Extensions;
using Atlas.Legal.Configuration;
using Atlas.Legal.EntityFrameworkCore;
using Atlas.Legal.Web.Filters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Web.Http;
using MvcMovie.Controllers;

namespace Atlas.Legal.Web.Startup
{
    [DependsOn(
        typeof(LegalApplicationModule), 
        typeof(LegalEntityFrameworkCoreModule), 
        typeof(AbpAspNetCoreModule))]
    public class LegalWebModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public LegalWebModule(IHostingEnvironment env)
        {
            _appConfiguration = AppConfigurations.Get(env.ContentRootPath, env.EnvironmentName);
        }

        public override void PreInitialize()
        {
            Configuration.BackgroundJobs.IsJobExecutionEnabled = true;            
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(LegalConsts.ConnectionStringName);
            
            Configuration.Modules.AbpAspNetCore()
                .CreateControllersForAppServices(
                    typeof(LegalApplicationModule).GetAssembly()
                );

            //Customize GetStatusCode 
            var mMvcOptions = Configuration.Get<IOptions<MvcOptions>>().Value;
            var mAbpExceptionFilter = mMvcOptions.Filters.FirstOrDefault(f => (f as ServiceFilterAttribute)?.ServiceType == (typeof(AbpExceptionFilter)));
            mMvcOptions.Filters.Remove(mAbpExceptionFilter);
            mMvcOptions.Filters.AddService(typeof(CustomExceptionFilter));
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(LegalWebModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            base.PostInitialize();
            
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(LegalWebModule).Assembly);

            var mJobManager = IocManager.IocContainer.Resolve<TareasProgramadas.IJobManagerService>();
            mJobManager.ProgramarJobs();

        }
    }
}