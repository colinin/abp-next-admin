using LINGYUN.Abp.WebhooksManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace LINGYUN.Abp.WebhooksManagement;

public abstract class WebhooksManagementControllerBase : AbpControllerBase
{
    protected WebhooksManagementControllerBase()
    {
        LocalizationResource = typeof(WebhooksManagementResource);
    }
}
