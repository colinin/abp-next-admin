using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.FeatureManagement.Localization;

namespace LINGYUN.Abp.FeatureManagement.HttpApi;

public abstract class FeatureManagementControllerBase : AbpControllerBase
{
    protected FeatureManagementControllerBase()
    {
        LocalizationResource = typeof(AbpFeatureManagementResource);
    }
}
