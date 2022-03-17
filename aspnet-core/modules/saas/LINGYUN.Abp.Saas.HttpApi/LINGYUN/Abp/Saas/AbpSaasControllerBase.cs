using LINGYUN.Abp.Saas.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.Saas;
public abstract class AbpSaasControllerBase : AbpControllerBase
{
    protected AbpSaasControllerBase()
    {
        LocalizationResource = typeof(AbpSaasResource);
    }
}
