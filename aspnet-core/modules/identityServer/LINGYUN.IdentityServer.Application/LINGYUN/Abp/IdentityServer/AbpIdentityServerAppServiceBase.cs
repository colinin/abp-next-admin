using Volo.Abp.Application.Services;
using Volo.Abp.IdentityServer.Localization;

namespace LINGYUN.Abp.IdentityServer
{
    public abstract class AbpIdentityServerAppServiceBase : ApplicationService
    {
        protected AbpIdentityServerAppServiceBase()
        {
            LocalizationResource = typeof(AbpIdentityServerResource);
        }
    }
}
