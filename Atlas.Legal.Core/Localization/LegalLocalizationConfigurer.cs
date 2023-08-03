using System.Reflection;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Json;
using Abp.Reflection.Extensions;

namespace Atlas.Legal.Localization
{
    public static class LegalLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Languages.Add(new LanguageInfo("en", "English", "famfamfam-flags england", isDefault: true));

            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(LegalConsts.LocalizationSourceName,
                    new JsonEmbeddedFileLocalizationDictionaryProvider(
                        typeof(LegalLocalizationConfigurer).GetAssembly(),
                        "Atlas.Legal.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}