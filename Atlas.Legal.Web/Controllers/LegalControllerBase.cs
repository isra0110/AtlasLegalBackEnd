using Abp.AspNetCore.Mvc.Controllers;

namespace Atlas.Legal.Web.Controllers
{
    public abstract class LegalControllerBase: AbpController
    {
        protected LegalControllerBase()
        {
            LocalizationSourceName = LegalConsts.LocalizationSourceName;
        }
    }
}