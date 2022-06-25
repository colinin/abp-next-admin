using LINGYUN.Abp.TextTemplating.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.TextTemplating;

public abstract class AbpTextTemplatingControllerBase : AbpControllerBase
{
    protected AbpTextTemplatingControllerBase()
    {
        LocalizationResource = typeof(AbpTextTemplatingResource);
    }
}
