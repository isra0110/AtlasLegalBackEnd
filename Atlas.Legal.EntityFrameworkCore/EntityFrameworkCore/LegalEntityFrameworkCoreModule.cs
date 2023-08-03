using Abp.EntityFrameworkCore;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Atlas.Legal.EntityFrameworkCore
{
    [DependsOn(
        typeof(LegalCoreModule), 
        typeof(AbpEntityFrameworkCoreModule))]
    public class LegalEntityFrameworkCoreModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(LegalEntityFrameworkCoreModule).GetAssembly());
        }
    }
}