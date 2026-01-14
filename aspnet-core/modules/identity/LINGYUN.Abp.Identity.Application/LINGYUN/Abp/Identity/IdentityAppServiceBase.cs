using Volo.Abp.Application.Services;
using Volo.Abp.Identity.Localization;

namespace LINGYUN.Abp.Identity;
public abstract class IdentityAppServiceBase : ApplicationService
{
    protected IdentityAppServiceBase()
    {
        ObjectMapperContext = typeof(AbpIdentityApplicationModule);
        LocalizationResource = typeof(IdentityResource);
    }
}
