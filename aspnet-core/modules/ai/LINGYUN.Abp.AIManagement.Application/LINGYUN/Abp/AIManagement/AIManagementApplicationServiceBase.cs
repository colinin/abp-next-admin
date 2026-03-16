using LINGYUN.Abp.AIManagement.Localization;
using Volo.Abp.Application.Services;

namespace LINGYUN.Abp.AIManagement;
public abstract class AIManagementApplicationServiceBase : ApplicationService
{
    protected AIManagementApplicationServiceBase()
    {
        LocalizationResource = typeof(AIManagementResource);
        ObjectMapperContext = typeof(AbpAIManagementApplicationModule);
    }
}
