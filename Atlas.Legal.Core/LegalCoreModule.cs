using Abp.Modules;
using Abp.Reflection.Extensions;
using Atlas.Legal.Localization;

namespace Atlas.Legal
{
    public class LegalCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            LegalLocalizationConfigurer.Configure(Configuration.Localization);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(LegalCoreModule).GetAssembly());
        }
    }
}