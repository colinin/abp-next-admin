using LINGYUN.Abp.WebhooksManagement.Localization;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.WebhooksManagement;

public abstract class WebhooksManagementAppServiceBase : ApplicationService
{
    protected WebhooksManagementAppServiceBase()
    {
        LocalizationResource = typeof(WebhooksManagementResource);
        ObjectMapperContext = typeof(WebhooksManagementApplicationModule);
    }
}
