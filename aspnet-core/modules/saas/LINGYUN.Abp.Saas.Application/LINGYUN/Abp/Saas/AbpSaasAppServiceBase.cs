using LINGYUN.Abp.Saas.Localization;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.Saas;
public abstract class AbpSaasAppServiceBase : ApplicationService
{
    protected AbpSaasAppServiceBase()
    {
        ObjectMapperContext = typeof(AbpSaasApplicationModule);
        LocalizationResource = typeof(AbpSaasResource);
    }
}
