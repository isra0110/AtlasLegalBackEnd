using Abp.AutoMapper;
using Abp.Modules;
using Abp.Quartz;
using Abp.Reflection.Extensions;

namespace Atlas.Legal
{
    [DependsOn(
        typeof(LegalCoreModule), 
        typeof(AbpAutoMapperModule),
        typeof(AbpQuartzModule))]
    public class LegalApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(LegalApplicationModule).GetAssembly());

            Configuration.Modules.AbpAutoMapper().Configurators.Add(mapper =>
            {
                DtoMappings.Map(mapper);
            });
        }
    }
}