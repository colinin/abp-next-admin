using Volo.Abp.Application.Services;
using Volo.Abp.SettingManagement.Localization;

namespace LINGYUN.Abp.SettingManagement;
public abstract class SettingManagementAppServiceBase : ApplicationService
{
    protected SettingManagementAppServiceBase()
    {
        ObjectMapperContext = typeof(AbpSettingManagementApplicationModule);
        LocalizationResource = typeof(AbpSettingManagementResource);
    }
}
