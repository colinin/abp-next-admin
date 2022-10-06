using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.OpenIddict.Localization;

namespace LINGYUN.Abp.OpenIddict;

public abstract class OpenIddictControllerBase : AbpControllerBase
{
    protected OpenIddictControllerBase()
    {
        LocalizationResource = typeof(AbpOpenIddictResource);
    }
}
